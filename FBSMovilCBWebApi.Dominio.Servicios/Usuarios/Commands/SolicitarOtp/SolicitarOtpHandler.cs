using AutoMapper;
using FBS.Dominio.Modelos.CorreoElectronico;
using FBS.Dominio.Servicios.Interfaces.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Newtonsoft.Json;
using OtpNet;
using ServiciosFinancial;
using ServiciosFinancial.Models;
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
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IServicioCorreoElectronico _correoElectronico;
        private readonly IFBSCorresponsalesApi _servicioFinancial;
        private readonly byte[] _llave;

        public SolicitarOtpHandler(IMediator mediador, IRepositorioAgente repositorioAgente,
            IFBSCorresponsalesApi servicioFinancial, IRepositorioUsuario repositorioUsuario,
            IJsonConfiguracion jsonConfiguracion, IServicioCorreoElectronico correoElectronico)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _servicioFinancial = servicioFinancial;
            _repositorioUsuario = repositorioUsuario;
            _jsonConfiguracion = jsonConfiguracion;
            _correoElectronico = correoElectronico;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<bool> Handle(SolicitarOtpME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
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
            else
            {
                var cliente = await _servicioFinancial.Clientes.DevuelveDatosPersonaIdentificacionWithHttpMessagesAsync(new PorIdentificacionSocioME()
                {
                    Identificacion = request.Identificacion
                });
                cuentaDestino = cliente.Body.CorreoElectronico;
                nombreDestino = cliente.Body.Nombres + cliente.Body.Apellidos != null && cliente.Body.Apellidos != "" ? " " + cliente.Body.Apellidos : "";
            }
            if (cuentaDestino != "" && nombreDestino != "")
                try
                {
                    _correoElectronico.Enviar(new ModeloMensaje()
                    {
                        Asunto = "OTP Corresponsales Solidarios",
                        Mensaje = "Su OTP para realizar la Operación es: " + totp.ComputeTotp(),
                        DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo(){
                            Direccion=cuentaDestino,
                            Nombre = nombreDestino
                        }
                    }
                    });
                    await _repositorioUsuario.EliminarOtp(request.Identificacion);
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(request),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                }
                catch (Exception)
                {
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject("No se ha podido enviar el correo electrónico"),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                    throw new Exception("No se ha podido enviar el Correo Electrónico");

                }

            return true;
        }
    }
}
