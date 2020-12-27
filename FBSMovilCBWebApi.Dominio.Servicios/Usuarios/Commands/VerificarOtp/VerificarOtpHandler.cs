using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OtpNet;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class VerificarOtpHandler : IRequestHandler<VerificarOtpME, bool>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly byte[] _llave;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguracionCanal _configuracionCanal;

        public VerificarOtpHandler(
            IMediator mediador, 
            IRepositorioUsuario repositorioUsuario,
            IJsonConfiguracion jsonConfiguracion, 
            IRepositorioAgente repositorioAgente,
            IHttpContextAccessor httpContext,
            IConfiguracionCanal configuracionCanal)
        {
            _mediador = mediador;
            _repositorioUsuario = repositorioUsuario;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioAgente = repositorioAgente;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _httpContext = httpContext;
            _configuracionCanal = configuracionCanal;
        }

        public async Task<bool> Handle(VerificarOtpME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdVerificarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            await _mediador.Send(new VerificarAgenteME()
            {
                Imei = request.Imei,
                Mac = request.Mac,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                Usuario = request.Usuario,
                VerificarGeolocalizacion = false
            });            

            var otpReferenciaStr = await _repositorioUsuario.ReferenciaOtp(request.Usuario, request.Identificacion);

            var otpReferencia = JsonConvert.DeserializeObject<OtpReferencia>(otpReferenciaStr);

            if (otpReferencia == null)
            {
                return false;
            }

            var verificacion = false;

            try
            {
                var secretKey = Criptografia.EncryptStringToBytes_Aes(otpReferencia.Referencia, _llave, _llave);
                var tiempoVida = _jsonConfiguracion.TiempoVidaOtp;
                var totp = new Totp(secretKey, tiempoVida);
                long tiempoVerificacion = 0;
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(request),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdVerificarOtp").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
                var vw = new VerificationWindow(1, 1);
                if (otpReferencia.IntentosFallidos < _configuracionCanal.Negocio.NumeroMaximoIntentosFallidosOtp)
                {
                    verificacion = totp.VerifyTotp(request.Otp, out tiempoVerificacion, vw);
                }
                else
                {
                    throw new ExcepcionApp("Ha excedido el máximo de intentos para verificar el otp.");
                }
            }
            catch (ExcepcionApp e)
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(e.Message),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
                throw e;
            }
            catch (Exception e)
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(e.Message),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
                throw new Exception("Ha ocurrido un error al verificar el otp.");
            }            

            if (verificacion)
            {
                await _repositorioUsuario.EliminarOtp(request.Identificacion);
                return true;
            }
            else
            {
                otpReferencia.IntentosFallidos++;
                await _repositorioUsuario.SalvarOtp(request.Usuario, request.Identificacion, JsonConvert.SerializeObject(otpReferencia));
                if (otpReferencia.IntentosFallidos >= _configuracionCanal.Negocio.NumeroMaximoIntentosFallidosOtp)
                {
                    throw new ExcepcionApp("Ha excedido el máximo de intentos para verificar el otp.");
                }

            }
            return false;
        }
    }
}
