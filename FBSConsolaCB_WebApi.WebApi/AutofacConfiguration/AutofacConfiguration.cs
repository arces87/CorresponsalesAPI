using Autofac;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using FBS_Core.Identity.Domain.Services.Seguridad;
using FBS_Core.Identity.Infraestructure.Interfaces;
using FBS_Core.Identity.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.Domain.Services.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Services.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
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
            builder.RegisterType<AreaTrabajoRepository>().As<IAreaTrabajoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OficinaRepository>().As<IOficinaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CargoRepository>().As<ICargoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CorresponsalRepository>().As<ICorresponsalRepository>().InstancePerLifetimeScope();

            builder.RegisterType<TipoCatalogoRepository>().As<ITipoCatalogoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CatalogoRepository>().As<ICatalogoRepository>().InstancePerLifetimeScope();

            builder.RegisterType<FBSConsolaCBContext>().As<FBSIdentityDBContext>().SingleInstance();
        }

        protected void LoadServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<PermisoService>().As<IPermisoService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();

            builder.RegisterType<EmpresaService>().As<IEmpresaService>().InstancePerLifetimeScope();
            builder.RegisterType<AreaTrabajoService>().As<IAreaTrabajoService>().InstancePerLifetimeScope();
            builder.RegisterType<OficinaService>().As<IOficinaService>().InstancePerLifetimeScope();
            builder.RegisterType<CargoService>().As<ICargoService>().InstancePerLifetimeScope();
            builder.RegisterType<CorresponsalService>().As<ICorresponsalService>().InstancePerLifetimeScope();

            builder.RegisterType<TipoCatalogoService>().As<ITipoCatalogoService>().InstancePerLifetimeScope();
            builder.RegisterType<CatalogoService>().As<ICatalogoService>().InstancePerLifetimeScope();
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadServices(builder);
            LoadRepositories(builder);
        }
    }
}
