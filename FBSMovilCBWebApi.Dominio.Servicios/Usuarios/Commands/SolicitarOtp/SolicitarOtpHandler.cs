using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Dominio.Modelos.CorreoElectronico;
using FBS.Dominio.Servicios.Interfaces.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Newtonsoft.Json;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarOtpHandler : IRequestHandler<SolicitarOtpME, bool>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IServicioCorreoElectronico _correoElectronico;
        private readonly byte[] _llave;

        public SolicitarOtpHandler(IMediator mediador, IRepositorioAgente repositorioAgente, IMapper mapper,
            IJsonConfiguracion jsonConfiguracion, IRepositorioGeolocalizacion repositorioGeolocalizacion, IServicioCorreoElectronico correoElectronico)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
            _correoElectronico = correoElectronico;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<bool> Handle(SolicitarOtpME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                IdUsuario = request.Usuario,
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            var secretKey = Criptografia.EncryptStringToBytes_Aes(request.Identificacion, _llave, _llave);
            var tiempoVida = _jsonConfiguracion.TiempoVidaOtp;
            var totp = new Totp(secretKey, tiempoVida);
            var cuentaDestino = "";
            var nombreDestino = "";
            if (request.ParaAgente)
            {
                var agente = await _repositorioAgente.GetForUserName(request.Usuario);
                cuentaDestino = agente.Usuario.Email;
                nombreDestino = agente.NombreAgente;
            }
            if (cuentaDestino != "" && nombreDestino != "")
                _correoElectronico.Enviar(new ModeloMensaje()
                {
                    Asunto = "OTP Banca Movil",
                    Mensaje = "Su OTP para realizar la Operación es: " + totp.ComputeTotp(),
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo(){
                            Direccion=cuentaDestino,
                            Nombre = nombreDestino
                        }
                    }
                });
            await _mediador.Send(new CrearLogME()
            {
                IdUsuario = request.Usuario,
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            return true;
        }
    }
}
