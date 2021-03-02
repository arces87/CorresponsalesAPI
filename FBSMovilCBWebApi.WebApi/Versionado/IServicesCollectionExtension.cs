using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ArchitectTest.Versionado
{
    public static class IServicesCollectionExtension
    {
        public static IServiceCollection AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v''VVVV'";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, List<string> apiVersion)
        {
            services.AddSwaggerGen(c =>
            {
                apiVersion.ForEach(version =>
                {
                    c.SwaggerDoc($"v{version}", new OpenApiInfo { Title = "FBS Corresponsales Solidario Móvil Api", Version = $"v{version}" });
                });

                c.ResolveConflictingActions(a => a.First());
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVerionWithExactValueInPath>();
            });

            return services;
        }
    }

}
