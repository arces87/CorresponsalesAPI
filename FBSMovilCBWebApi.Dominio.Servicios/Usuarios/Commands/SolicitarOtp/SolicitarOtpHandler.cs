using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OtpNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarOtpHandler : IRequestHandler<SolicitarOtpME, SolicitarOtpMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IClientesApi _clienteApi;
        private readonly IMensajeriaSMSApi _mensajeriaApi;
        private readonly byte[] _llave;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public SolicitarOtpHandler(IMediator mediador,
            IRepositorioAgente repositorioAgente,
            IRepositorioDispositivoAgente repositorioDispositivoAgente,
            IRepositorioDispositivo repositorioDispositivo,
            IClientesApi clienteApi,
            IMensajeriaSMSApi mensajeriaApi,
            IRepositorioUsuario repositorioUsuario,
            IJsonConfiguracion jsonConfiguracion,
            IHttpContextAccessor httpContext,
            IApiKeyGenerator apiKeyGenerator)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _repositorioDispositivo = repositorioDispositivo;
            _clienteApi = clienteApi;
            _mensajeriaApi = mensajeriaApi;
            _repositorioUsuario = repositorioUsuario;
            _jsonConfiguracion = jsonConfiguracion;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _httpContext = httpContext;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<SolicitarOtpMS> Handle(SolicitarOtpME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var user = await _repositorioUsuario.GetPorUsuario(request.Usuario);

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

            try
            {
                GenerarOtp(request, out tiempoVidaMinutos, out otp, out referencia);

                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(otp),
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

                throw new Exception("Ha ocurrido un error al generar el otp.");
            }

            var otpReferencia = new OtpReferencia()
            {
                Referencia = referencia
            };

            var cuentaDestino = "";
            //var nombreDestino = "";
            //string userName = "";
            //string identificacion = "";
            //int tipoIdentificacion = 0;

            var fechaActualEmail = DateTime.Now.ToString("dd/MM/yyyy/ H:mm");
            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy");
            var horaActual = DateTime.Now.ToString("H:mm");
            var nombreDestino = user.NombreMostrar;

            var respuestaOTP = new SolicitarOtpMS
            {
                OtpGenerado = true,
                NotificationEmailErrorMensaje = "",
                NotificationSMSErrorMensaje = "",
            };

            if (request.ParaAgente)
            {
                cuentaDestino = agente.Usuario.Email;
                //nombreDestino = agente.NombreAgente;
            }
            else
            {
                //var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
                //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

                try
                {
                    await _mediador.Send(new CrearLogME()
                    {
                        //JsonLog = JsonConvert.SerializeObject(customHeaders),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    var porIdentificacionSocioME = new PorIdentificacionSocioME()
                    {
                        Identificacion = request.Identificacion,
                        SecuencialTipoIdentificacion = request.SecuencialTipoIdentificacion
                    };

                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(porIdentificacionSocioME),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    var cliente = await _clienteApi.ClientesDevuelveDatosPersonaIdentificacionAsync(porIdentificacionSocioME);
                  
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(cliente),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });

                    if (cliente != null)
                    {
                        cuentaDestino = cliente.CorreoElectronico;
                        //nombreDestino = String.IsNullOrEmpty(cliente.Nombres + cliente.Apellidos) ? "" : cliente.Nombres + cliente.Apellidos;
                    }
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
                    respuestaOTP.NotificationEmailErrorMensaje = "No se ha podido enviar el Correo Electrónico con el otp solicitado, no fue posible optener los datos del cliente.";
                }               
            }

            if (String.IsNullOrEmpty(cuentaDestino) || String.IsNullOrEmpty(nombreDestino))
            {
                respuestaOTP.NotificationEmailError = true;
                respuestaOTP.NotificationEmailErrorMensaje = "No fue posible notificar el otp generado, el agente no cuenta con un email o un nombre definido";
            } else
            {
                try
                {
                    await EnviarEmail(tiempoVidaMinutos, otp, cuentaDestino, nombreDestino, fechaActualEmail);
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
            }

            if (request.SecuencialTipoIdentificacion <= 0 || String.IsNullOrEmpty(request.Identificacion))
            {
                respuestaOTP.NotificationSMSError = true;
                respuestaOTP.NotificationSMSErrorMensaje = "No se ha podido enviar el SMS con el OTP solicitado.";
            } else
            {
                try
                {
                    var dispositivoagente = await _repositorioDispositivoAgente.GetForAgente(agente.Id.ToString());
                    var dispositivo = await _repositorioDispositivo.Get(dispositivoagente.DispositivoId.ToString());
                    var respuesta = await EnviarSMS(agente.Usuario.UserName, request.Identificacion, request.SecuencialTipoIdentificacion, tiempoVidaMinutos, otp);

                    await _mediador.Send(new CrearLogME()
                    {
                        //JsonLog = JsonConvert.SerializeObject(respuesta),
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
            }

            await _repositorioUsuario.SalvarOtp(request.Usuario, request.Identificacion, JsonConvert.SerializeObject(otpReferencia));

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });

            if (respuestaOTP.NotificationEmailError && respuestaOTP.NotificationSMSError)
            {
                throw new ExcepcionApp("No fue posible notificar el otp generado.");
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

        private async Task<EnvioSMSMS> EnviarSMS(
            string UserName, 
            string Identificacion, 
            int TipoIdentificacion, 
            int tiempoVidaMinutos, 
            string otp)
        {
            var smsTemplate = File.ReadAllText("Resources/SmsTemplate/template_otp.txt");

            smsTemplate = smsTemplate.Replace("[:OTP:]", otp)
                .Replace("[:TIEMPO_VIDA:]", $"{tiempoVidaMinutos.ToString()}");

            var mensajeSMS = new EnvioSMSME()
            {
                CodigoUsuarioCorresponsal = UserName,
                MensajeTexto = smsTemplate,
                NumeroIdentificacion = Identificacion,
                SecuencialTipoIdentificacion = TipoIdentificacion
            };

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(mensajeSMS),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });           

            var respuesta = await _mensajeriaApi.MensajeriaSMSEnvioSMSAsync(mensajeSMS);
            return respuesta;
        }

        private async Task EnviarEmail(int tiempoVidaMinutos, string otp, string cuentaDestino, string nombreDestino, string fechaActual)
        {
            var emailTemplate = File.ReadAllText("Resources/EmailTemplate/index_otp.html");

            emailTemplate = emailTemplate.Replace("[:NOMBRECORRESPONSAL:]", nombreDestino)
                        .Replace("[:OTP:]", otp)
                        .Replace("[:TIEMPO_VIDA:]", $"{tiempoVidaMinutos.ToString()} minutos")
                        .Replace("[:FECHAACTUAL:]", fechaActual);

            var email = new EnviarCorreoElectronicoME
            {
                Asunto = "OTP Corresponsales Solidarios",
                Mensaje = emailTemplate,
                DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = cuentaDestino,
                            Nombre = nombreDestino
                        }
                    }
            };

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(email),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });

            await _mediador.Publish(email);
        }
    }
}
