using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace ArchitectTest.Versionado
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseSwaggerApiVersion(this IApplicationBuilder app, IWebHostEnvironment env, List<string> apiVersion)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    apiVersion.ForEach(x =>
                    {
                        c.SwaggerEndpoint($"/swagger/v{x}/swagger.json", $"FBS Corresponsales Solidario Móvil Api{x}");
                    });
                });

            }

            return app;
        }
    }
}
