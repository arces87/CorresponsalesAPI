using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Excepciones;
using FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.WebApi.ManejadorExcepciones
{
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public HttpStatusCodeExceptionMiddleware(
            RequestDelegate next, 
            ILoggerFactory loggerFactory, 
            IMediator mediador, 
            IServiceScopeFactory serviceProvider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _mediador = (IMediator)serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IMediator));
            _jsonConfiguracion = (IJsonConfiguracion)serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IJsonConfiguracion));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var mensajeSalida = "";
                var notificar = false;
                var guardarLog = true;
                var response = context.Response;
                response.ContentType = @"text/plain";
                response.StatusCode = StatusCodes.Status400BadRequest;
                switch (error)
                {
                   
                    case ExcepcionApp e:
                        mensajeSalida = e.Message;
                        notificar = true;
                        break;
                    case HttpOperationException e:
                        var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>(e.Response.Content);
                        //var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>((ex.InnerException as HttpOperationException).Response.Content);

                        mensajeSalida = "Sistema no disponible en estos momentos, por favor intentarlo más tardes o comunicarse con su supervisor";

                        if (mensaje.InnerException != null) 
                        { 
                            if (mensaje.InnerException.ExceptionMessage.Substring(0, 4) == "CNB-")
                            {
                                int length = mensaje.InnerException.ExceptionMessage.Length - 4;
                                mensajeSalida = mensaje.InnerException.ExceptionMessage.Substring(4, length);
                                notificar = true;
                            }
                        }                        
                        break;
                    case SqlException e:
                        mensajeSalida = "Ah ocurrido un error al ejecutar la operación en la Base de Datos.";
                        notificar = true;
                        break;
                    case DbException e:
                        mensajeSalida = "No fue posible conectarse a la Base de Datos.";
                        notificar = true;
                        guardarLog = false;
                        break;
                    case SmtpException e:
                        mensajeSalida = "No fue posible enviar el email.";
                        notificar = false;
                        break;
                    case SocketException e:
                        mensajeSalida = "Ha ocurrido un error al establecer la conexión con el servicio.";
                        break;
                    default:                        
                        mensajeSalida = error.Message;  
                        break;
                }

                //if (notificar && context.User.Identity.Name != null)
                //{
                //    await _mediador.Publish(new NotificaSupervisorME
                //    {
                //        MensajeExcepcion = mensajeSalida,
                //        IdUsuario = context.User.Identity.Name
                //    });
                //}

                if (guardarLog)
                {
                    var errorDetalle = new DetalleError
                    {
                        CodigoEstado = response.StatusCode,
                        Mensaje = mensajeSalida,
                        MensajeExcepcion = error.Message,
                        SeguimientoPila = error.StackTrace
                    };
                    await _mediador.Send(new CrearLogME()
                    {
                        JsonLog = JsonConvert.SerializeObject(errorDetalle),
                        IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                        IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                    });
                }
                await response.WriteAsync(mensajeSalida);
                return;

            }
        }
    }
}
