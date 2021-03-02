using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.WebApi.Versionado
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseSwaggerApiVersion(this IApplicationBuilder app, IWebHostEnvironment env, List<string> apiVersion)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger(o => o.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
            {
                apiVersion.ForEach(x =>
                {
                    c.SwaggerEndpoint($"/swagger/v{x}/swagger.json", $"FBS Corresponsales Solidario Web Api {x}");
                });
            });

            return app;
        }
    }
}
