using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.Dominio.Servicios.MapperConfigurator;
using FBSConsolaCB_WebApi.WebApi.AutofacConfiguration;
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
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace FBSConsolaCB_WebApi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContextoFBSConsolaCB>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FBSConsolaCB_WebApi.WebApi")));

            #region Swagger Configuration
            services.AddSwaggerGen(swagger =>
            {
                var contact = new Contact() { Name = SwaggerConfiguration.SwaggerConfiguration.ContactName, Url = SwaggerConfiguration.SwaggerConfiguration.ContactUrl };
                swagger.SwaggerDoc(SwaggerConfiguration.SwaggerConfiguration.DocNameV1,
                                   new Info
                                   {
                                       Title = SwaggerConfiguration.SwaggerConfiguration.DocInfoTitle,
                                       Version = SwaggerConfiguration.SwaggerConfiguration.DocInfoVersion,
                                       Description = SwaggerConfiguration.SwaggerConfiguration.DocInfoDescription,
                                       Contact = contact
                                   }
                                    );
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                swagger.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                swagger.AddSecurityRequirement(security);
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

            //services.AddAuthorization(options =>
            //{
            //    var _contexto = services.BuildServiceProvider().GetService<GeNeDBContext>();
            //    foreach (var item in _contexto.Permisos)
            //    {
            //        options.AddPolicy(item.Nombre,
            //            policy => policy.RequireClaim(item.Descripcion, item.Identificador));
            //    }

            //});
            #endregion
            services.AddCors();
            services.AddAutoMapper(typeof(FBSConsolaCBAutoMapperConfiguratorProfile));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Configuracion Inyeccion Dependencia 
            ConfiguracionInyeccionDependencia.LoadRepositories(services);
            ConfiguracionInyeccionDependencia.LoadServices(services);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region Swagger Configuration
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.SwaggerConfiguration.EndpointDescription);
            });
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

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
