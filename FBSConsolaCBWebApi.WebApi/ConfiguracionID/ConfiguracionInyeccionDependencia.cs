using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Identidad.Infraestructura.Repositorio;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Canales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Nomenclador;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServiciosFinancial;
using System;
using System.Linq;
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
            services.AddScoped<IRepositorioCanal, RepositorioCanal>();

            services.AddScoped<IRepositorioTipoCatalogo, RepositorioTipoCatalogo>();
            services.AddScoped<IRepositorioCatalogo, RepositorioCatalogo>();


            services.AddScoped<IRepositorioDispositivo, RepositorioDispositivo>();
            services.AddScoped<IRepositorioImagen, RepositorioImagen>();
            services.AddScoped<IRepositorioImagenGeolocalizacion, RepositorioImagenGeolocalizacion>();
            services.AddScoped<IRepositorioGeolocalizacion, RepositorioGeolocalizacion>();
            services.AddScoped<IRepositorioLog, RepositorioLog>();


            services.AddScoped<IRepositorioAgente, RepositorioAgente>();
            services.AddScoped<IRepositorioCuenta, RepositorioCuenta>();
            services.AddScoped<IRepositorioAlerta, RepositorioAlerta>();
            services.AddScoped<IRepositorioTransaccion, RepositorioTransaccion>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();

            services.AddSingleton<IApiKeyGenerator>(new ApiKeyGenerator("b9be8fe4-d8a5-4fb8-a591-2ed86af6ffde"));
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuracion)
        {
            var contexto = services.BuildServiceProvider().GetService<ContextoFBSConsolaCB>();
            var canal = contexto.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
            var jsonConfiguracion = JsonConvert.DeserializeObject<JsonConfiguracion>(canal.JsonConfiguracion);
            jsonConfiguracion.IdCanal = configuracion["CanalBase"];

            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(canal.JsonNegocio);

            PreprarIntanciaConfiguracionCanal(services, configuracion);

            PrepararIntanciaParamtetrizacionCanal(services, configuracion);

            ConfigurarApiCoreFinanciero(services, jsonConfiguracion);

            ConfigurarIdentity(services, jsonNegocio);
        }

        private static void ConfigurarApiCoreFinanciero(IServiceCollection services, JsonConfiguracion jsonConfiguracion)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "UrlFinancial").Valor),
            };
            services.AddSingleton<IFBSCorresponsalesApi>(new FBSCorresponsalesApi(httpClient, false));
        }

        private static void ConfigurarIdentity(IServiceCollection services, JsonNegocioMS jsonNegocio)
        {
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(jsonNegocio.TiempoBloqueo);
                opt.Lockout.MaxFailedAccessAttempts = jsonNegocio.NumeroMaximoIntentosFallidos;
            });
        }

        private static void PrepararIntanciaParamtetrizacionCanal(IServiceCollection services, IConfiguration configuracion)
        {
            services.AddScoped<IConfiguracionCanal>((serviceProvider =>
            {
                var contextoScoped = services.BuildServiceProvider().GetService<ContextoFBSConsolaCB>();
                var canalObjeto = contextoScoped.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
                var jsonConfiguracionObjeto = JsonConvert.DeserializeObject<JsonConfiguracion>(canalObjeto.JsonConfiguracion);
                jsonConfiguracionObjeto.IdCanal = configuracion["CanalBase"];

                var configuracionCanal = new ConfiguracionCanal
                {
                    IdCanal = configuracion["CanalBase"],
                    Configuracion = jsonConfiguracionObjeto,
                    Negocio = JsonConvert.DeserializeObject<JsonNegocio>(canalObjeto.JsonNegocio)
                };

                return configuracionCanal;
            }));
        }

        private static void PreprarIntanciaConfiguracionCanal(IServiceCollection services, IConfiguration configuracion)
        {
            services.AddScoped<IJsonConfiguracion>((serviceProvider =>
            {
                var contextoScoped = services.BuildServiceProvider().GetService<ContextoFBSConsolaCB>();
                var canalScoped = contextoScoped.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
                var jsonConfiguracionScoped = JsonConvert.DeserializeObject<JsonConfiguracion>(canalScoped.JsonConfiguracion);
                jsonConfiguracionScoped.IdCanal = configuracion["CanalBase"];

                return jsonConfiguracionScoped;
            }));
        }
    }
}
