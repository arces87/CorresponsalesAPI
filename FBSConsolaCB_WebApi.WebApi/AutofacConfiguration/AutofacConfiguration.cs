using Autofac;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using FBS_Core.Identity.Domain.Services.Seguridad;
using FBS_Core.Identity.Infraestructure.Interfaces;
using FBS_Core.Identity.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;

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
            builder.RegisterType<FBSConsolaCBContext>().As<FBSIdentityDBContext>().SingleInstance();
        }

        protected void LoadServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<PermisoService>().As<IPermisoService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadServices(builder);
            LoadRepositories(builder);
        }
    }
}
