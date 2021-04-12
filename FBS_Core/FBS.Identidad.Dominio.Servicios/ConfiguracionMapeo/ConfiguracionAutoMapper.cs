using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.Menus.Commands;
using FBS.Identidad.Dominio.Servicios.Menus.Queries;
using FBS.Identidad.Dominio.Servicios.Roles.Commands;
using FBS.Identidad.Dominio.Servicios.Roles.Queries;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Identidad.Dominio.Servicios.Usuarios.Queries;
using System;

namespace FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionAutoMapper : Profile
    {
        public ConfiguracionAutoMapper()
        {

            #region Menu
            CreateMap<CrearMenuME, Menu>();
            CreateMap<ModificarMenuME, Menu>();
            CreateMap<EliminarMenuME, Menu>();

            CreateMap<Menu, ObtenerMenuMS>();
            CreateMap<Menu, ModeloListaMenu>();
            CreateMap<Menu, ModeloListaMenuUsuario>();
            #endregion

            #region Rol
            CreateMap<CrearRolME, Rol>()
                .ForMember(r => r.Name, opt => opt.MapFrom(m => m.Nombre))
                .ForMember(r => r.NormalizedName, opt => opt.MapFrom(m => m.Nombre));
            CreateMap<ModificarRolME, Rol>()
                .ForMember(r => r.Name, opt => opt.MapFrom(m => m.Nombre))
                .ForMember(r => r.NormalizedName, opt => opt.MapFrom(m => m.Nombre));
            CreateMap<EliminarRolME, Rol>();

            CreateMap<Rol, ModeloObtenerRol>()
                .ForMember(r => r.Nombre, opt => opt.MapFrom(m => m.Name));
            CreateMap<Menu, ObtenerRolMenu>();
            CreateMap<Rol, ModeloListaRol>()
                .ForMember(r => r.Nombre, opt => opt.MapFrom(m => m.Name));

            CreateMap<ListaRolME, ModeloPaginacion>();
            #endregion

            #region Usuario
            CreateMap<CrearUsuarioME, Usuario>()
                .ForMember(r => r.UserName, opt => opt.MapFrom(m => m.Usuario))
                .ForMember(r => r.PasswordHash, opt => opt.MapFrom(m => m.Contrasenna))
                .ForMember(r => r.Email, opt => opt.MapFrom(m => m.CorreoElectronico))
                .ForMember(r => r.Operadora, opt => opt.MapFrom(m => new Catalogo() { Id = new Guid(m.IdOperadora) }))
                .ForMember(r => r.PhoneNumber, opt => opt.MapFrom(m => m.Telefono));
            CreateMap<ModificarUsuarioME, Usuario>()
                .ForMember(r => r.UserName, opt => opt.MapFrom(m => m.Usuario))
                .ForMember(r => r.PasswordHash, opt => opt.MapFrom(m => m.Contrasenna))
                .ForMember(r => r.Email, opt => opt.MapFrom(m => m.CorreoElectronico))
                .ForMember(r => r.Operadora, opt => opt.MapFrom(m => new Catalogo() { Id = new Guid(m.IdOperadora) }));
            CreateMap<EliminarUsuarioME, Usuario>();
            CreateMap<Rol, LoginUsuarioRol>()
                .ForMember(r => r.Nombre, opt => opt.MapFrom(m => m.Name));
            CreateMap<Rol, ObtenerUsuarioRol>()
                .ForMember(r => r.Nombre, opt => opt.MapFrom(m => m.Name));
            CreateMap<Usuario, ModeloLoginUsuario>()
               .ForMember(r => r.IdUsuario, opt => opt.MapFrom(m => m.Id))
               .ForMember(r => r.Usuario, opt => opt.MapFrom(m => m.UserName))
               .ForMember(r => r.CorreoElectronico, opt => opt.MapFrom(m => m.Email));
            CreateMap<Usuario, ModeloObtenerUsuario>()
                .ForMember(r => r.Usuario, opt => opt.MapFrom(m => m.UserName))
                .ForMember(r => r.CorreoElectronico, opt => opt.MapFrom(m => m.Email))
                .ForMember(r => r.IdOperadora, opt => opt.MapFrom(m => m.Operadora.Id))
                .ForMember(r => r.NombreOperadora, opt => opt.MapFrom(m => m.Operadora.Nombre))
                .ForMember(r => r.Telefono, opt => opt.MapFrom(m => m.PhoneNumber));
            CreateMap<Usuario, ModeloListaUsuario>()
                .ForMember(r => r.Usuario, opt => opt.MapFrom(m => m.UserName))
                .ForMember(r => r.CorreoElectronico, opt => opt.MapFrom(m => m.Email))
                .ForMember(r => r.IdOperadora, opt => opt.MapFrom(m => m.Operadora.Id))
                .ForMember(r => r.NombreOperadora, opt => opt.MapFrom(m => m.Operadora.Nombre));
            #endregion
        }
    }
}
