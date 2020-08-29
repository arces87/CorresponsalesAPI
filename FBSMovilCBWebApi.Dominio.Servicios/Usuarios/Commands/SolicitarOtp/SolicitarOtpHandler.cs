using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OtpNet;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarOtpHandler : IRequestHandler<SolicitarOtpME, SolicitarOtpMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IFBSCorresponsalesApi _servicioFinancial;
        private readonly byte[] _llave;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public SolicitarOtpHandler(IMediator mediador,
            IRepositorioAgente repositorioAgente,
            IFBSCorresponsalesApi servicioFinancial,
            IRepositorioUsuario repositorioUsuario,
            IJsonConfiguracion jsonConfiguracion,
            IHttpContextAccessor httpContext,
            IApiKeyGenerator apiKeyGenerator)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _servicioFinancial = servicioFinancial;
            _repositorioUsuario = repositorioUsuario;
            _jsonConfiguracion = jsonConfiguracion;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _httpContext = httpContext;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<SolicitarOtpMS> Handle(SolicitarOtpME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
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

            int tiempoVidaMinutos;
            string otp;
            string referencia;
            GenerarOtp(request, out tiempoVidaMinutos, out otp, out referencia);

            var cuentaDestino = "";
            var nombreDestino = "";

            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy/ H:mm");

            var respuestaOTP = new SolicitarOtpMS
            {
                OtpGenerado = true,
                NotificationEmailErrorMensaje = "",
                NotificationSMSErrorMensaje = "",
            };

            if (request.ParaAgente)
            {
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
            {
                try
                {
                    await EnviarEmail(tiempoVidaMinutos, otp, cuentaDestino, nombreDestino, fechaActual);
                }
                catch (Exception e)
                {
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(e.Message),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                    respuestaOTP.NotificationEmailError = true;
                    respuestaOTP.NotificationEmailErrorMensaje = "No se ha podido enviar el Correo Electrónico con el OTP solicitado.";
                }

                try
                {
                    var respuesta = await EnviarSMS(agente, tiempoVidaMinutos, otp, nombreDestino, fechaActual);

                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(respuesta),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                }
                catch (Exception e)
                {
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(e.Message),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    respuestaOTP.NotificationSMSError = true;
                    respuestaOTP.NotificationSMSErrorMensaje = "No se ha podido enviar el SMS con el OTP solicitado.";
                }

                await _repositorioUsuario.SalvarOtp(request.Usuario, agente.Identificacion, referencia);

                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(request),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
            } else
            {
                throw new Exception("No fue posible notificar el otp generado, el agente no cuenta con un email o un nombre defino.");
            }

            if (respuestaOTP.NotificationEmailError && respuestaOTP.NotificationSMSError)
            {
                throw new Exception("No fue posible notificar el otp generado.");
            }

            return respuestaOTP;
        }

        private void GenerarOtp(SolicitarOtpME request, out int tiempoVidaMinutos, out string otp, out string referencia)
        {
            referencia = Guid.NewGuid().ToString();
            var secretKey = Criptografia.EncryptStringToBytes_Aes(referencia, _llave, _llave);
            var tiempoVida = _jsonConfiguracion.TiempoVidaOtp;
            tiempoVidaMinutos = tiempoVida / 60;
            var totp = new Totp(secretKey, tiempoVida);
            otp = totp.ComputeTotp();
        }

        private async Task<Microsoft.Rest.HttpOperationResponse<EnvioSMSMS>> EnviarSMS(FBSConsolaCBWebApi.DAL.Corresponsales.Agente agente, int tiempoVidaMinutos, string otp, string nombreDestino, string fechaActual)
        {
            var smsTemplate = File.ReadAllText("Resources/SmsTemplate/template_otp.txt");

            smsTemplate = smsTemplate.Replace("[:NOMBRECORRESPONSAL:]", nombreDestino)
                .Replace("[:OTP:]", otp)
                .Replace("[:TIEMPO_VIDA:]", $"{tiempoVidaMinutos.ToString()} m")
                .Replace("[:FECHAACTUAL:]", fechaActual);

            var mensajeSMS = new EnvioSMSME()
            {
                CodigoUsuarioCorresponsal = agente.Usuario.UserName,
                MensajeTexto = smsTemplate,
                NumeroIdentificacion = agente.Identificacion,
                SecuencialTipoIdentificacion = agente.TipoIdentificacion
            };

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(mensajeSMS),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });

            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var respuesta = await _servicioFinancial.MensajeriaSMS.EnvioSMSWithHttpMessagesAsync(mensajeSMS, customHeaders);
            return respuesta;
        }

        private async Task EnviarEmail(int tiempoVidaMinutos, string otp, string cuentaDestino, string nombreDestino, string fechaActual)
        {
            var emailTemplate = File.ReadAllText("Resources/EmailTemplate/index_otp.html");

            emailTemplate = emailTemplate.Replace("[:NOMBRECORRESPONSAL:]", nombreDestino)
                        .Replace("[:OTP:]", otp)
                        .Replace("[:TIEMPO_VIDA:]", $"{tiempoVidaMinutos.ToString()} minutos")
                        .Replace("[:FECHAACTUAL:]", fechaActual);

            await _mediador.Publish(new EnviarCorreoElectronicoME
            {
                Asunto = "OTP Corresponsales Solidarios",
                Mensaje = emailTemplate,
                DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = cuentaDestino,
                            Nombre = nombreDestino
                        }
                    }
            });
        }
    }
}
