using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
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
            catch (HttpOperationException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = @"text/plain";
                var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>(ex.Response.Content);
                await context.Response.WriteAsync(mensaje.InnerException.ExceptionMessage);
                return;
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                if (ex.InnerException is HttpOperationException)
                {
                    context.Response.ContentType = @"text/plain";
                    var mensaje = JsonConvert.DeserializeObject<ExcepcionFinancial>((ex.InnerException as HttpOperationException).Response.Content);
                    await context.Response.WriteAsync(mensaje.InnerException.ExceptionMessage);
                }
                else
                {
                    context.Response.ContentType = @"application/json";
                    await context.Response.WriteAsync(ex.Message);
                }

                return;
            }
        }
    }
}
