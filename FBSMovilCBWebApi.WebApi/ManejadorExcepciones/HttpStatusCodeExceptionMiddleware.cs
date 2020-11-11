using FBS.Infraestructura.Utiles;
using FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands;
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



        public HttpStatusCodeExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IMediator mediador, IServiceScopeFactory serviceProvider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            //_mediador = mediador;
            _mediador = (IMediator)serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IMediator));
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
                var notificar = true;
                var response = context.Response;
                response.ContentType = @"text/plain";
                switch (error)
                {
                    case ExcepcionApp e:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        mensajeSalida = e.Message;
                        break;
                    case HttpOperationException e:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>(e.Response.Content);
                        //var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>((ex.InnerException as HttpOperationException).Response.Content);

                        mensajeSalida = "Sistema no disponible en estos momentos, por favor intentarlo más tardes o comunicarse con su supervisor";

                        if (mensaje.InnerException != null) 
                        { 
                            if (mensaje.InnerException.ExceptionMessage.Substring(0, 4) == "CNB-")
                            {
                                int length = mensaje.InnerException.ExceptionMessage.Length - 4;
                                mensajeSalida = mensaje.InnerException.ExceptionMessage.Substring(4, length);
                            }
                        }                        
                        break;
                    case SqlException e:
                        mensajeSalida = "Ah ocurrido un error al ejecutar la operación en la Base de Datos.";
                        notificar = true;
                        break;
                    case DbException e:
                        mensajeSalida = "No fue posible conectarse a la Base de Datos";
                        notificar = false;
                        break;
                    case SmtpException e:
                        mensajeSalida = "No fue posible enviar el email";
                        notificar = false;
                        break;
                    case SocketException e:
                        mensajeSalida = "Ha ocurrido un error al establecer la conexión con el servicio";
                        break;
                    default:
                        mensajeSalida = "Ha ocurrido un error, contacte al administrador";
                        break;
                }

                if (notificar && context.User.Identity.Name != null)
                {
                    await _mediador.Publish(new NotificaSupervisorME
                    {
                        MensajeExcepcion = mensajeSalida,
                        IdUsuario = context.User.Identity.Name
                    });
                }
                await response.WriteAsync(mensajeSalida);
                return;

            }
        }
    }
}
