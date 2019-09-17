using FBS.DAL.Nomenclador;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands;
using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSConsolaCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSConsolaCB() : base()
        {


            #region Dispositivos
            CreateMap<CrearDispositivoME, Dispositivo>()
                .ForMember(m => m.Marca, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdMarca) }))
                .ForMember(m => m.SistemaOperativo, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdSistemaOperativo) }));
            CreateMap<ModificarDispositivoME, Dispositivo>()
                .ForMember(m => m.Marca, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdMarca) }))
                .ForMember(m => m.SistemaOperativo, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdSistemaOperativo) }));
            CreateMap<EliminarDispositivoME, Dispositivo>();

            CreateMap<Dispositivo, ObtenerDispositivoMS>()
               .ForMember(m => m.IdMarca, opt => opt.MapFrom(d => d.Marca.Id))
               .ForMember(m => m.NombreMarca, opt => opt.MapFrom(d => d.Marca.Nombre))
               .ForMember(m => m.IdSistemaOperativo, opt => opt.MapFrom(d => d.SistemaOperativo.Id))
               .ForMember(m => m.NombreSistemaOperativo, opt => opt.MapFrom(d => d.SistemaOperativo.Nombre));
            CreateMap<Dispositivo, ModeloListaDispositivo>()
               .ForMember(m => m.IdMarca, opt => opt.MapFrom(d => d.Marca.Id))
               .ForMember(m => m.NombreMarca, opt => opt.MapFrom(d => d.Marca.Nombre))
               .ForMember(m => m.IdSistemaOperativo, opt => opt.MapFrom(d => d.SistemaOperativo.Id))
               .ForMember(m => m.NombreSistemaOperativo, opt => opt.MapFrom(d => d.SistemaOperativo.Nombre));
            CreateMap<Imagen, ObtenerDispositivoImagen>()
              .ForMember(m => m.Imagen, opt => opt.MapFrom(d => d.DireccionImagen));
            #endregion

            #region Dispositivos
            CreateMap<CrearAgenteME, Agente>()
                .ForMember(m => m.Estado, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdEstado) }))
                .ForMember(m => m.Usuario, opt => opt.MapFrom(d => new Usuario() { Id = d.IdUsuario }))
                .ForMember(m => m.Supervisor, opt => opt.MapFrom(d => new Usuario() { Id = d.IdSupervisor }));
            CreateMap<ModificarAgenteME, Agente>()
                .ForMember(m => m.Estado, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdEstado) }))
                .ForMember(m => m.Usuario, opt => opt.MapFrom(d => new Usuario() { Id = d.IdUsuario }))
                .ForMember(m => m.Supervisor, opt => opt.MapFrom(d => new Usuario() { Id = d.IdSupervisor }));
            CreateMap<EliminarAgenteME, Agente>();

            CreateMap<Agente, ObtenerAgenteMS>()
               .ForMember(m => m.IdEstado, opt => opt.MapFrom(d => d.Estado.Id))
               .ForMember(m => m.NombreEstado, opt => opt.MapFrom(d => d.Estado.Nombre))
               .ForMember(m => m.IdUsuario, opt => opt.MapFrom(d => d.Usuario.Id))
               .ForMember(m => m.NombreUsuario, opt => opt.MapFrom(d => d.Usuario.UserName))
               .ForMember(m => m.IdSupervisor, opt => opt.MapFrom(d => d.Supervisor.Id))
               .ForMember(m => m.NombreSupervisor, opt => opt.MapFrom(d => d.Supervisor.UserName));
            CreateMap<Agente, ModeloListaAgente>()
              .ForMember(m => m.IdEstado, opt => opt.MapFrom(d => d.Estado.Id))
               .ForMember(m => m.NombreEstado, opt => opt.MapFrom(d => d.Estado.Nombre))
               .ForMember(m => m.IdUsuario, opt => opt.MapFrom(d => d.Usuario.Id))
               .ForMember(m => m.NombreUsuario, opt => opt.MapFrom(d => d.Usuario.UserName))
               .ForMember(m => m.IdSupervisor, opt => opt.MapFrom(d => d.Supervisor.Id))
               .ForMember(m => m.NombreSupervisor, opt => opt.MapFrom(d => d.Supervisor.UserName));
            #endregion

            #region Catalogo
            CreateMap<CrearCatalogoME, Catalogo>()
                .ForMember(m => m.TipoCatalogo, opt => opt.MapFrom(d => new TipoCatalogo() { Id = new Guid(d.IdTipoCatalogo) }));
            CreateMap<ModificarCatalogoME, Catalogo>()
                .ForMember(m => m.TipoCatalogo, opt => opt.MapFrom(d => new TipoCatalogo() { Id = new Guid(d.IdTipoCatalogo) }));
            CreateMap<EliminarCatalogoME, Catalogo>();

            CreateMap<Catalogo, ObtenerCatalogoMS>()
               .ForMember(m => m.IdTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Id))
               .ForMember(m => m.NombreTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Nombre));
            CreateMap<Catalogo, ModeloListaCatalogo>()
               .ForMember(m => m.IdTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Id))
               .ForMember(m => m.NombreTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Nombre));
            #endregion

            #region Tipo Catalogo
            CreateMap<ModificarTipoCatalogoME, TipoCatalogo>();
            CreateMap<TipoCatalogo, ModeloListarTipoCatalogo>();
            CreateMap<TipoCatalogo, ObtenerTipoCatalogoMS>();
            #endregion


            #region Usuarios
            CreateMap<AutenticarUsuarioME, LoginUsuarioME>();
            CreateMap<ModeloLoginUsuario, AutenticarUsuarioMS>();
            CreateMap<LoginUsuarioRol, AutenticarUsuarioRol>();
            CreateMap<Agente, AutenticarUsuarioMS>()
                .ForMember(m => m.Usuario, opt => opt.Ignore())
                .ForMember(m => m.IdAgente, opt => opt.MapFrom(d => d.Id));
            #endregion

            #region Filtros
            CreateMap<ListarCatalogoME, ModeloPaginacion>();
            CreateMap<ListarTipoCatalogoME, ModeloPaginacion>();
            CreateMap<ListaDispositivoME, ModeloPaginacion>();
            CreateMap<ListaAgenteME, ModeloPaginacion>();
            #endregion
        }
    }
}
