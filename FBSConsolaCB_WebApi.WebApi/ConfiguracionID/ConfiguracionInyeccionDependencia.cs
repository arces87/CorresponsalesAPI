using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using FBS.Identidad.Dominio.Servicios.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.Dominio.Servicios.Consola;
using FBSConsolaCB_WebApi.Dominio.Servicios.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Seguridad;
using FBSConsolaCB_WebApi.Dominio.Servicios.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Repositories.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Repositories.Nomenclador;
using Microsoft.Extensions.DependencyInjection;

namespace FBSConsolaCB_WebApi.WebApi.AutofacConfiguration
{
    public static class ConfiguracionInyeccionDependencia
    {
        internal static void LoadRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioRol, RepositorioRol>();
            services.AddScoped<IRepositorioMenu, RepositorioMenu>();
            services.AddScoped<IRepositorioPermiso, RepositorioPermiso>();

            services.AddScoped<IRepositorioEmpresa, RepositorioEmpresa>();
            services.AddScoped<IRepositorioOficina, RepositorioOficina>();
            services.AddScoped<IRepositorioPersona, RepositorioPersona>();

            services.AddScoped<IRepositorioTipoCatalogo, RepositorioTipoCatalogo>();
            services.AddScoped<IRepositorioCatalogo, RepositorioCatalogo>();

            services.AddScoped<IRepositorioDispositivo, DispositivoRepositorio>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();
        }

        internal static void LoadServices(IServiceCollection services)
        {
            services.AddScoped<IServicioUsuario, ServicioUsuario>();
            services.AddScoped<IServicioRol, ServicioRol>();
            services.AddScoped<IServicioPermiso, ServicioPermiso>();
            services.AddScoped<IServicioMenu, ServicioMenu>();
            services.AddScoped<IServicioUsuarioLocal, ServicioUsuarioLocal>();

            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IOficinaService, OficinaService>();
            services.AddScoped<IPersonaService, PersonaService>();

            services.AddScoped<ITipoCatalogoService, TipoCatalogoService>();
            services.AddScoped<ICatalogoService, CatalogoService>();

            services.AddScoped<IServicioDispositivo, ServicioDispositivo>();
        }
    }
}
