using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using FBSConsolaCBWebApi.DAL;
using FBSMovilCBWebApi.Dominio.Servicios.ConfiguracionMapeo;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FBS.Identidad.DAL.Seguridad;
using FFBSMovilCBWebApi.WebApi.AutofacConfiguration;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBSMovilCBWebApi.WebApi.ManejadorExcepciones;
using FBS.Dominio.Servicios.GestionFicheros;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using FBSMovilCBWebApi.Dominio.Servicios.Canal;
using Microsoft.OpenApi.Models;
using FBS.Infraestructura.Utiles;
using FBSMovilCBWebApi.WebApi.Versionado;

namespace FBSMovilCBWebApi.WebApi
{
    public class Startup
    {
        List<string> apiVersion = new List<string>() { "1.0", "2.0" };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddHttpClient();
            services.AddDbContext<ContextoFBSConsolaCB>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FBSMovilCBWebApi.WebApi")));

            #region Swagger Configuration
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "AutorizacionFBS.Api", Version = "v1" });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Cabecera de Autorización JWT usando Bearer Ejemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                     {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,

                                },
                                new List<string>()
                            }
                     });
            });

            #endregion

            #region Authentication Configuration
            services.AddIdentity<Usuario, Rol>()
                .AddEntityFrameworkStores<ContextoFBSConsolaCB>()
                .AddDefaultTokenProviders();
            //Add Jwt Token Handler
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion
            services.AddCors();
            services.AddAutoMapper(typeof(ConfiguracionPerfilAutoMapperFBSMovilCB));
            services.AddMediatR(
                typeof(ConfiguracionPerfilAutoMapperFBSMovilCB).Assembly, 
                typeof(ConfiguracionAutoMapper).Assembly, 
                typeof(GuardarFicheroME).Assembly,
                typeof(ObtenerRequisitoCanalME).Assembly);
            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
            });


            #region Include Versioning

            services.AddSwagger(apiVersion);
            services.AddApiVersioning();

            #endregion

            //services.AddApiVersioning(config =>
            //{
            //    config.DefaultApiVersion = new ApiVersion(1, 0);
            //    config.AssumeDefaultVersionWhenUnspecified = true;
            //    config.ReportApiVersions = true;
            //});


            #region Configuracion Inyeccion Dependencia 
            ConfiguracionInyeccionDependencia.LoadRepositories(services);
            ConfiguracionInyeccionDependencia.LoadServices(services, Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseResponseCompression();

            #region Versioning Swagger
            app.UseSwaggerApiVersion(env, apiVersion);
            #endregion

            //#region Swagger Configuration
            //app.UseSwagger(o => o.SerializeAsV2 = true);

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint(SwaggerConfiguration.SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.SwaggerConfiguration.EndpointDescription);
            //});
            //#endregion

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHsts();
            app.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
            #region Cors Configuration
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            #endregion

            app.UseAuthentication()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
