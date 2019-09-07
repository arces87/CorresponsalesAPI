using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Canales;
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
            services.AddScoped<IRepositorioPersona, RepositorioPersona>();

            services.AddScoped<IRepositorioTipoCatalogo, RepositorioTipoCatalogo>();
            services.AddScoped<IRepositorioCatalogo, RepositorioCatalogo>();


            services.AddScoped<IRepositorioDispositivo, RepositorioDispositivo>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuration)
        {
            var httpClient = new HttpClient();
            var configuracionFinancial = configuration.GetSection("FinancialServerConfig");
            httpClient.BaseAddress = new Uri(configuracionFinancial["DireccionIp"] + ":" + configuracionFinancial["Puerto"]);
            services.AddSingleton<IFBSBancaApi>(new FBSBancaApi(httpClient, false));
        }
    }
}
