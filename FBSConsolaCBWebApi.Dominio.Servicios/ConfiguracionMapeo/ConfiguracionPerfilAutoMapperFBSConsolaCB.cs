using FBS.DAL.Nomenclador;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries;
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


            #region Catalogo
            CreateMap<CrearCatalogoCommand, Catalogo>()
                .ForMember(m => m.TipoCatalogo, opt => opt.MapFrom(d => new TipoCatalogo() { Id = new Guid(d.IdTipoCatalogo) }));
            CreateMap<ModificarCatalogoCommand, Catalogo>()
                .ForMember(m => m.TipoCatalogo, opt => opt.MapFrom(d => new TipoCatalogo() { Id = new Guid(d.IdTipoCatalogo) }));
            CreateMap<EliminarCatalogoCommand, Catalogo>();

            CreateMap<Catalogo, ObtenerModeloCatalogo>()
               .ForMember(m => m.IdTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Id))
               .ForMember(m => m.NombreTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Nombre));
            CreateMap<Catalogo, ModeloObtenerDetalleListaCatalogo>()
               .ForMember(m => m.IdTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Id))
               .ForMember(m => m.NombreTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Nombre));
            #endregion
            #region Tipo Catalogo
            CreateMap<ModificarTipoCatalogoCommand, TipoCatalogo>();
            CreateMap<TipoCatalogo, ModeloObtenerDetalleListaTipoCatalogo>();
            CreateMap<TipoCatalogo, ObtenerModeloTipoCatalogo>();
            #endregion


            #region Usuarios
            CreateMap<AutenticarUsuarioCommand, LoginUsuarioCommand>();
            CreateMap<ModeloUsuarioAutenticado, ModeloAutenticacion>();
            CreateMap<ModeloUsuarioRol, ModeloRolAutenticacion>();
            CreateMap<Agente, ModeloAutenticacion>()
                .ForMember(m => m.Usuario, opt => opt.Ignore())
                .ForMember(m => m.IdAgente, opt => opt.MapFrom(d => d.Id));
            #endregion

            #region Filtros
            CreateMap<ObtenerListaCatalogoQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaTipoCatalogoQuery, ModeloPaginacion>();
            #endregion
        }
    }
}
