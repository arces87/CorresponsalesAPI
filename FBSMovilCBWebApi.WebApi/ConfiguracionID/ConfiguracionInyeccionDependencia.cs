using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using FBS.Identidad.Dominio.Servicios.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Consola;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FFBSMovilCBWebApi.WebApi.AutofacConfiguration
{
    public static class ConfiguracionInyeccionDependencia
    {
        internal static void LoadRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioRol, RepositorioRol>();
            services.AddScoped<IRepositorioMenu, RepositorioMenu>();
            services.AddScoped<IRepositorioPermiso, RepositorioPermiso>();

            services.AddScoped<IRepositorioLog, RepositorioLog>();
            services.AddScoped<IRepositorioAlerta, RepositorioAlerta>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServicioUsuario, ServicioUsuario>();
            services.AddScoped<IServicioRol, ServicioRol>();
            services.AddScoped<IServicioPermiso, ServicioPermiso>();
            services.AddScoped<IServicioMenu, ServicioMenu>();
        }
    }
}
