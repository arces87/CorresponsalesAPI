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
    public class SolicitarOtpHandler : IRequestHandler<SolicitarOtpME, bool>
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

        public async Task<bool> Handle(SolicitarOtpME request, CancellationToken cancellationToken)
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

            var secretKey = Criptografia.EncryptStringToBytes_Aes(request.Identificacion, _llave, _llave);
            var tiempoVida = _jsonConfiguracion.TiempoVidaOtp;
            var tiempoVidaMinutos = tiempoVida / 60;
            var totp = new Totp(secretKey, tiempoVida);
            var cuentaDestino = "";
            var nombreDestino = "";

            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy/ H:mm");

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

                    var emailTemplate = File.ReadAllText("Resources/EmailTemplate/index _otp.html");

                    emailTemplate = emailTemplate.Replace("[:NOMBRECORRESPONSAL:]", nombreDestino)
                                .Replace("[:OTP:]", totp.ComputeTotp())
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
                catch (Exception e)
                {
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(e.Message),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                    throw new Exception("No se ha podido enviar el Correo Electrónico.");
                }

                try
                {

                    var smsTemplate = File.ReadAllText("Resources/SmsTemplate/template_otp.txt");

                    smsTemplate = smsTemplate.Replace("[:NOMBRECORRESPONSAL:]", nombreDestino)
                        .Replace("[:OTP:]", totp.ComputeTotp())
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

                    return true;
                    //throw new Exception("No se ha podido enviar el sms.");                 
                }

                await _repositorioUsuario.EliminarOtp(request.Identificacion);
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(request),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarOtp").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
            }
                         
            return true;
        }
    }
}
