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
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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
            services.AddScoped<IRepositorioTransaccionRetiro, RepositorioTransaccionRetiro>();
            services.AddScoped<IRepositorioDispositivoAgente, RepositorioDispositivoAgente>();

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
            var urlCoreFinanciero = jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "UrlFinancial").Valor;

            var certificadoByte = GetResourceAsBytes("FBSConsolaCBWebApi.WebApi.ConfiguracionID.certinfrahttps.pfx");
            var certificado = new X509Certificate2(certificadoByte, "Lc1234*");

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var confCommand =
                new Corresponsales.Command.Client.Configuration
                {
                    BasePath = urlCoreFinanciero,
                    //ClientCertificates = new X509CertificateCollection(new X509Certificate[] { certificado })
                };

            var confQuery =
                new Corresponsales.Query.Client.Configuration
                {
                    BasePath = urlCoreFinanciero,
                    //ClientCertificates = new X509CertificateCollection(new X509Certificate[] { certificado })
                };

            services.AddSingleton<Corresponsales.Command.Api.ICaptacionesVistaApi>(new Corresponsales.Command.Api.CaptacionesVistaApi(confCommand));
            services.AddSingleton<Corresponsales.Command.Api.ICarteraApi>(new Corresponsales.Command.Api.CarteraApi(confCommand));
            services.AddSingleton<IClienteApi>(new ClienteApi(confCommand));
            services.AddSingleton<Corresponsales.Command.Api.IGeneralesApi>(new Corresponsales.Command.Api.GeneralesApi(confCommand));
            services.AddSingleton<IUsuarioApi>(new UsuarioApi(confCommand));

            services.AddSingleton<Corresponsales.Query.Api.ICaptacionesVistaApi>(new Corresponsales.Query.Api.CaptacionesVistaApi(confQuery));
            services.AddSingleton<Corresponsales.Query.Api.ICarteraApi>(new Corresponsales.Query.Api.CarteraApi(confQuery));
            services.AddSingleton<Corresponsales.Query.Api.IGeneralesApi>(new Corresponsales.Query.Api.GeneralesApi(confQuery));
            services.AddSingleton<IPersonaApi>(new PersonaApi(confQuery));

            //services.AddSingleton<IClientesApi>(new ClientesApi(conf));
            //services.AddSingleton<IAfectacionApi>(new AfectacionApi(conf));
            //services.AddSingleton<ICuentasApi>(new CuentasApi(conf));
            //services.AddSingleton<IMensajeriaSMSApi>(new MensajeriaSMSApi(conf));
            //services.AddSingleton<IPrestamosApi>(new PrestamosApi(conf));
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
