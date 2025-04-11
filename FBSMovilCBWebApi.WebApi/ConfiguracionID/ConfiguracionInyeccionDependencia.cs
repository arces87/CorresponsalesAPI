using Corresponsales.AccesoFinancial.Api;
using Corresponsales.Command.Api;
using Corresponsales.Query.Api;
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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

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
            services.AddScoped<IRepositorioTransaccionRetiro, RepositorioTransaccionRetiro>();
            services.AddScoped<IRepositorioCuenta, RepositorioCuenta>();
            services.AddScoped<IRepositorioCanal, RepositorioCanal>();
            services.AddScoped<IRepositorioDispositivoAgente, RepositorioDispositivoAgente>();
            services.AddScoped<IRepositorioDispositivo, RepositorioDispositivo>();

            services.AddScoped<ContextoFBSIdentidad, ContextoFBSConsolaCB>();

            services.AddSingleton<IApiKeyGenerator>(new ApiKeyGenerator("b9be8fe4-d8a5-4fb8-a591-2ed86af6ffde"));
        }

        internal static void LoadServices(IServiceCollection services, IConfiguration configuracion)
        {

            var contexto = services.BuildServiceProvider().GetService<ContextoFBSConsolaCB>();
            var canal = contexto.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
            var jsonConfiguracion = JsonConvert.DeserializeObject<JsonConfiguracion>(canal.JsonConfiguracion);
            jsonConfiguracion.IdCanal = configuracion["CanalBase"];


            PreprarIntanciaConfiguracionCanal(services, configuracion);

            PrepararIntanciaParamtetrizacionCanal(services, configuracion);

            ConfigurarApiCoreFinanciero(services, jsonConfiguracion);
        }

        private static void ConfigurarApiCoreFinanciero(IServiceCollection services, JsonConfiguracion jsonConfiguracion)
        {
            var urlCoreFinanciero = jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "UrlFinancial").Valor;  
                       
            var confCommand =
                new Corresponsales.Command.Client.Configuration
                {
                    BasePath = urlCoreFinanciero + "Corresponsales.Command",                
                    //BasePath = "http://186.5.29.68:9503/Corresponsales.Command"
                    //BasePath = "https://localhost:62796",
                };
            
            var confQuery =
                new Corresponsales.Query.Client.Configuration
                {
                    BasePath = urlCoreFinanciero + "Corresponsales.Query",                    
                    //BasePath = "http://186.5.29.68:9503/Corresponsales.Query",                    
                    //BasePath = "https://localhost:62798",                    
                };

            services.AddSingleton<Corresponsales.Command.Api.ICaptacionesVistaApi>(new Corresponsales.Command.Api.CaptacionesVistaApi(confCommand));
            services.AddSingleton<Corresponsales.Command.Api.ICarteraApi>(new Corresponsales.Command.Api.CarteraApi(confCommand));
            services.AddSingleton<IClienteApi>(new ClienteApi(confCommand));            
            services.AddSingleton<Corresponsales.Command.Api.IFacilitoApi>(new Corresponsales.Command.Api.FacilitoApi(confCommand));            
            services.AddSingleton<Corresponsales.Command.Api.IGeneralesApi>(new Corresponsales.Command.Api.GeneralesApi(confCommand));
            services.AddSingleton<IUsuarioApi>(new UsuarioApi(confCommand));

            services.AddSingleton<Corresponsales.Query.Api.ICaptacionesVistaApi>(new Corresponsales.Query.Api.CaptacionesVistaApi(confQuery));
            services.AddSingleton<Corresponsales.Query.Api.ICarteraApi>(new Corresponsales.Query.Api.CarteraApi(confQuery));
            services.AddSingleton<Corresponsales.Query.Api.IFacilitoApi>(new Corresponsales.Query.Api.FacilitoApi(confQuery));
            services.AddSingleton<Corresponsales.Query.Api.IGeneralesApi>(new Corresponsales.Query.Api.GeneralesApi(confQuery));            
            services.AddSingleton<IPersonaApi>(new PersonaApi(confQuery));           

            services.AddTransient<FinancialRequestInfo>();
            services.AddSingleton(x =>
            {
                var opts = x.GetRequiredService<IConfiguration>();

                var token = new AuthInfo
                {
                    BaseUrl = opts["FinancialOptions:ServiceUrl"],
                    LoginEndpoint = opts["FinancialOptions:LoginEndpoint"],
                    RefreshEndpoint = opts["FinancialOptions:RefreshEndpoint"]                    
                };               

                return token;
            });

            ServiceProviderFactory.SetServiceProvider(services.BuildServiceProvider());
        }

        public static byte[] GetResourceAsBytes(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var resFilestream = assembly.GetManifestResourceStream(resourceName);

            if (resFilestream == null) return null;

            byte[] array = new byte[resFilestream.Length];
            resFilestream.Read(array, 0, array.Length);
            return array;
        }            

        private static void ConfigurarIdentity(IServiceCollection services, JsonNegocioMS jsonNegocio)
        {
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(36500);
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
                var canal= contextoScoped.Canales.FirstOrDefault(c => c.Id == new Guid(configuracion["CanalBase"]));
                var jsonConfiguracionScoped = JsonConvert.DeserializeObject<JsonConfiguracion>(canal.JsonConfiguracion);
                jsonConfiguracionScoped.IdCanal = configuracion["CanalBase"];


                var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(canal.JsonNegocio);

                ConfigurarIdentity(services, jsonNegocio);

                return jsonConfiguracionScoped;
            }));
        }
    }
}
