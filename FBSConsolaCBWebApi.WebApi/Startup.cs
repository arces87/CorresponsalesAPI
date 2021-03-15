using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using FBS.Identidad.DAL.Seguridad;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.Dominio.Servicios.ConfiguracionMapeo;
using FBSConsolaCBWebApi.WebApi.AutofacConfiguration;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBS.Dominio.Servicios.GestionFicheros;
using FBSConsolaCBWebApi.WebApi.ManejadorExcepciones;
using Microsoft.OpenApi.Models;
using FBS.Infraestructura.Utiles;
using FBSConsolaCBWebApi.WebApi.Versionado;

namespace FBSConsolaCBWebApi.WebApi
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

            services.AddHttpClient();
            services.AddDbContext<ContextoFBSConsolaCB>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FBSConsolaCBWebApi.WebApi")));

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
            services.AddAutoMapper(typeof(ConfiguracionPerfilAutoMapperFBSConsolaCB));
            services.AddMediatR(typeof(CrearCatalogoME).Assembly, typeof(ConfiguracionAutoMapper).Assembly, typeof(GuardarFicheroME).Assembly);
            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
            });


            #region Include Versioning

            services.AddSwagger(apiVersion);
            services.AddApiVersioning();

            #endregion       

            #region Configuracion Inyeccion Dependencia 
            ConfiguracionInyeccionDependencia.LoadRepositories(services);
            ConfiguracionInyeccionDependencia.LoadServices(services, Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            #region Versioning Swagger
            app.UseSwaggerApiVersion(env, apiVersion);
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<HttpStatusCodeExceptionMiddleware>();

            #region Statics Files Configurations
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            #endregion

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
