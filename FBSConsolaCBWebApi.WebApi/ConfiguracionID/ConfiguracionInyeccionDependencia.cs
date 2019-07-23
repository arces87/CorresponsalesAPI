using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using FBS.Identidad.Dominio.Servicios.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Dominio.Servicios.Consola;
using FBSConsolaCBWebApi.Dominio.Servicios.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Consola;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Nomenclador;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Seguridad;
using FBSConsolaCBWebApi.Dominio.Servicios.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Consola;
using FBSConsolaCBWebApi.Infraestructure.Repositories.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Nomenclador;
using Financial_Services_Banca;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace FBSConsolaCBWebApi.WebApi.AutofacConfiguration
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

            services.AddScoped<IRepositorioDispositivo, RepositorioDispositivo>();
            services.AddScoped<IRepositorioLimiteExistencia, RepositorioLimiteExistencia>();
            services.AddScoped<IRepositorioLimiteTransaccional, RepositorioLimiteTransaccional>();
            services.AddScoped<IRepositorioLog, RepositorioLog>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServicioUsuario, ServicioUsuario>();
            services.AddScoped<IServicioRol, ServicioRol>();
            services.AddScoped<IServicioPermiso, ServicioPermiso>();
            services.AddScoped<IServicioMenu, ServicioMenu>();
            services.AddScoped<IServicioUsuarioLocal, ServicioUsuarioLocal>();

            services.AddScoped<IServicioEmpresa, ServicioEmpresa>();
            services.AddScoped<IServicioOficina, ServicioOficina>();
            services.AddScoped<IServicioPersona, ServicioPersona>();

            services.AddScoped<IServicioTipoCatalogo, ServicioTipoCatalogo>();
            services.AddScoped<IServicioCatalogo, ServicioCatalogo>();

            services.AddScoped<IServicioDispositivo, ServicioDispositivo>();
            services.AddScoped<IServicioLimiteExistencia, ServicioLimiteExistencia>();
            services.AddScoped<IServicioLimiteTransaccional, ServicioLimiteTransaccional>();
            services.AddScoped<IServicioLog, ServicioLog>();

            var httpClient = new HttpClient();
            var configuracionFinancial = configuration.GetSection("FinancialServerConfig");
            httpClient.BaseAddress = new Uri(configuracionFinancial["DireccionIp"] + ":" + configuracionFinancial["Puerto"]);
            services.AddSingleton<IFBSBancaApi>(new FBSBancaApi(httpClient, false));
        }
    }
}
