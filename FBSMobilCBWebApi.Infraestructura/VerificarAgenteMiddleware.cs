using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace FBSMobilCBWebApi.Infraestructura
{
    public class VerificarAgenteMiddleware
    {
        private readonly RequestDelegate _next;

        public VerificarAgenteMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            var Imei = context.Request.Query["Imei"];
           
            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
