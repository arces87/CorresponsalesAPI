using Autofac;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using FBS_Core.Identity.Domain.Services.Seguridad;
using FBS_Core.Identity.Infraestructure.Interfaces;
using FBS_Core.Identity.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.Domain.Services.Consola;
using FBSConsolaCB_WebApi.Domain.Services.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Consola;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Seguridad;
using FBSConsolaCB_WebApi.Domain.Services.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Repositories.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Repositories.Nomenclador;

namespace FBSConsolaCB_WebApi.WebApi.AutofacConfiguration
{
    public class AutofacConfiguration : Module
    {
        private void LoadRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PermisoRepository>().As<IPermisoRepository>().InstancePerLifetimeScope();

            builder.RegisterType<EmpresaRepository>().As<IEmpresaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OficinaRepository>().As<IOficinaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PersonaRepository>().As<IPersonaRepository>().InstancePerLifetimeScope();

            builder.RegisterType<TipoCatalogoRepository>().As<ITipoCatalogoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CatalogoRepository>().As<ICatalogoRepository>().InstancePerLifetimeScope();

            builder.RegisterType<DispositivoRepository>().As<IDispositivoRepository>().InstancePerLifetimeScope();

            builder.RegisterType<FBSConsolaCBContext>().As<FBSIdentityDBContext>().InstancePerLifetimeScope();
        }

        protected void LoadServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<PermisoService>().As<IPermisoService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<UsuarioService>().As<IUsuarioService>().InstancePerLifetimeScope();

            builder.RegisterType<EmpresaService>().As<IEmpresaService>().InstancePerLifetimeScope();
            builder.RegisterType<OficinaService>().As<IOficinaService>().InstancePerLifetimeScope();
            builder.RegisterType<PersonaService>().As<IPersonaService>().InstancePerLifetimeScope();

            builder.RegisterType<TipoCatalogoService>().As<ITipoCatalogoService>().InstancePerLifetimeScope();
            builder.RegisterType<CatalogoService>().As<ICatalogoService>().InstancePerLifetimeScope();

            builder.RegisterType<DispositivoService>().As<IDispositivoService>().InstancePerLifetimeScope();
            
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadServices(builder);
            LoadRepositories(builder);
        }
    }
}
