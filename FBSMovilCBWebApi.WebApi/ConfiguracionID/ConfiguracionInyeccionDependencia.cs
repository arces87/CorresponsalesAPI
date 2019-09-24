using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Dominio.Servicios.Interfaces.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Canales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;

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

            services.AddScoped<IRepositorioAlerta, RepositorioAlerta>();
            services.AddScoped<IRepositorioLog, RepositorioLog>();
            services.AddScoped<IRepositorioGeolocalizacion, RepositorioGeolocalizacion>();
            services.AddScoped<IRepositorioAgente, RepositorioAgente>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuracion)
        {
            var configuracionCorreo = configuracion.GetSection("ConfiguracionCorreo");
            var servidorSmtp = configuracionCorreo["ServidorSmtp"];
            var puertoSmtp = int.Parse(configuracionCorreo["PuertoSmtp"]);
            var usuario = configuracionCorreo["Usuario"];
            var contrasenna = configuracionCorreo["Password"];
            services.AddSingleton<IServicioCorreoElectronico>(new ServicioCorreoElectronico(servidorSmtp, puertoSmtp, usuario, contrasenna));

            var _contexto = services.BuildServiceProvider().GetService<ContextoFBSConsolaCB>();
            var canal = _contexto.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
            var jsonConfiguracion = JsonConvert.DeserializeObject<JsonConfiguracion>(canal.JsonConfiguracion);
            services.AddSingleton<IJsonConfiguracion>(jsonConfiguracion);
        }
    }
}
