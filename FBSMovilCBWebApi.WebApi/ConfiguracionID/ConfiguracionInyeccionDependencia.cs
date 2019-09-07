using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Dominio.Servicios.Interfaces.CorreoElectronico;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
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
            var configuracionCorreo = configuration.GetSection("ConfiguracionCorreo");
            var servidorSmtp = configuracionCorreo["ServidorSmtp"];
            var puertoSmtp = int.Parse(configuracionCorreo["PuertoSmtp"]);
            var usuario = configuracionCorreo["Usuario"];
            var contrasenna = configuracionCorreo["Password"];
            services.AddSingleton<IServicioCorreoElectronico>(new ServicioCorreoElectronico(servidorSmtp, puertoSmtp, usuario, contrasenna));
        }
    }
}
