using FBS.Infraestructura;
using FBS.Infraestructura.Excepciones;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.WebApi.ManejadorExcepciones
{
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;


        public HttpStatusCodeExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorDetalle = new DetalleError()
                {
                    MensajeExcepcion = ex.Message
                };

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    _logger.LogError($"Ha ocurrido un error: {contextFeature.Error}");
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                switch (ex)
                {                     
                    case HttpOperationException e:
                        
                        var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>((ex.InnerException as HttpOperationException).Response.Content);
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        errorDetalle.Mensaje = mensaje.InnerException.ExceptionMessage;
                        break;
                    case ExcepcionApp e:
                        var excepcionApp = ex as ExcepcionApp;

                        if (excepcionApp.TipoError == TipoError.EmailNotification)
                        {
                            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        }
                        errorDetalle.Mensaje = ex.Message;
                        break;
                    case SqlException e:
                        errorDetalle.Mensaje = "Ah ocurrido un error al ejecutar la operación en la Base de Datos.";
                        break;
                    case DbException e:
                        errorDetalle.Mensaje = "Ah ocurrido un error al ejecutar la operación en la Base de Datos.";
                        break; 
                    default:
                        errorDetalle.Mensaje = "Ha ocurrido un error, contacte al administrador";
                        break;
                }

                errorDetalle.CodigoEstado = context.Response.StatusCode;
                await context.Response.WriteAsync(errorDetalle.Mensaje);

                return;
            }
        }
    }
}
