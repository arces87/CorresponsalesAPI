using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Canales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Nomenclador;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServiciosFinancial;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FFBSMovilCBWebApi.WebApi.AutofacConfiguration
{
    public static class ConfiguracionInyeccionDependencia
    {
        internal static void LoadRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioRol, RepositorioRol>();
            services.AddScoped<IRepositorioMenu, RepositorioMenu>();

            services.AddScoped<IRepositorioCatalogo, RepositorioCatalogo>();

            services.AddScoped<IRepositorioAlerta, RepositorioAlerta>();
            services.AddScoped<IRepositorioLog, RepositorioLog>();
            services.AddScoped<IRepositorioGeolocalizacion, RepositorioGeolocalizacion>();
            services.AddScoped<IRepositorioAgente, RepositorioAgente>();
            services.AddScoped<IRepositorioTransaccion, RepositorioTransaccion>();
            services.AddScoped<IRepositorioCuenta, RepositorioCuenta>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();

            services.AddSingleton<IApiKeyGenerator>(new ApiKeyGenerator("b9be8fe4-d8a5-4fb8-a591-2ed86af6ffde"));
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuracion)
        {
            var _contexto = services.BuildServiceProvider().GetService<ContextoFBSConsolaCB>();
            var canal = _contexto.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
            var jsonConfiguracion = JsonConvert.DeserializeObject<JsonConfiguracion>(canal.JsonConfiguracion);
            jsonConfiguracion.IdCanal = configuracion["CanalBase"];
            services.AddSingleton<IJsonConfiguracion>(jsonConfiguracion);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "UrlFinancial").Valor),
            };
            services.AddSingleton<IFBSCorresponsalesApi>(new FBSCorresponsalesApi(httpClient, false));
        }
    }
}
