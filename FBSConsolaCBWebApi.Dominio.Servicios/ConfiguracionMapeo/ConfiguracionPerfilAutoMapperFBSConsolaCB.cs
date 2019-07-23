using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using FBSConsolaCBWebApi.Dominio.Modelos.Consola;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Modelos.Nomenclador;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands;
using Financial_Services_Banca.Models;

namespace FBSConsolaCBWebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSConsolaCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSConsolaCB() : base()
        {
            CreateMap<Persona, ModeloPersona>().ReverseMap();
            CreateMap<InformacionPersonaMS, ModeloPersona>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Nombres.Split()[0]))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Nombres.Split().Length > 1 ? d.Nombres.Split()[1] : ""))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Apellidos.Split()[0]))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Apellidos.Split().Length > 1 ? d.Nombres.Split()[1] : ""))
                .ForMember(m => m.FechaNacimiento, opt => opt.MapFrom(d => d.FechaNacimientoCreacion));

            CreateMap<Corresponsal, ModeloCorresponsal>()
                .ForMember(m => m.EstaBloqueado, opt => opt.MapFrom(c => c.Persona.Usuario.LockoutEnabled)).ReverseMap();

            CreateMap<Supervisor, ModeloSupervisor>().ReverseMap();

            CreateMap<Empresa, ModeloEmpresa>().ReverseMap();

            CreateMap<Oficina, ModeloOficina>().ReverseMap();

            CreateMap<Catalogo, ModeloCatalogo>().ReverseMap();

            CreateMap<TipoCatalogo, ModeloTipoCatalogo>().ReverseMap();

            #region Dispositivo
            CreateMap<CrearDispositivoCommand, Dispositivo>()
               .ForMember(m => m.TipoDispositivo, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdTipoDispositivo }));
            CreateMap<EliminarDispositivoCommand, Dispositivo>();
            CreateMap<Dispositivo, ObtenerModeloDispositivo>()
                .ForMember(m => m.TipoDispositivo, opt => opt.MapFrom(d => d.TipoDispositivo.Nombre));
            CreateMap<Dispositivo, ModeloObtenerDetalleListaDispositivo>()
                .ForMember(m => m.TipoDispositivo, opt => opt.MapFrom(d => d.TipoDispositivo.Nombre));
            #endregion

            #region Catalogo
            CreateMap<CrearCatalogoCommand, Catalogo>()
                .ForMember(m => m.TipoCatalogo, opt => opt.MapFrom(d => new TipoCatalogo() { Id = d.IdTipoCatalogo }));
            CreateMap<ModificarCatalogoCommand, Catalogo>()
                .ForMember(m => m.TipoCatalogo, opt => opt.MapFrom(d => new TipoCatalogo() { Id = d.IdTipoCatalogo }));
            CreateMap<EliminarCatalogoCommand, Catalogo>();

            CreateMap<Catalogo, ObtenerModeloCatalogo>()
               .ForMember(m => m.IdTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Id))
               .ForMember(m => m.NombreTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Nombre));
            CreateMap<Catalogo, ModeloObtenerDetalleListaCatalogo>()
               .ForMember(m => m.IdTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Id))
               .ForMember(m => m.NombreTipoCatalogo, opt => opt.MapFrom(d => d.TipoCatalogo.Nombre));
            #endregion

            #region Alerta
            CreateMap<CrearAlertaCommand, Alerta>()
                .ForMember(m => m.Destinatario, opt => opt.MapFrom(d => new Persona() { Id = d.IdDestinatario }))
                .ForMember(m => m.Remitente, opt => opt.MapFrom(d => new Persona() { Id = d.IdRemitente }));
            CreateMap<ModificarAlertaCommand, Alerta>();
            CreateMap<EliminarAlertaCommand, Alerta>();

            CreateMap<Alerta, ObtenerModeloAlerta>();
            CreateMap<Alerta, ModeloObtenerDetalleListaAlerta>();
            #endregion

            #region Empresa
            CreateMap<CrearEmpresaCommand, Empresa>();
            CreateMap<ModificarEmpresaCommand, Empresa>();
            CreateMap<EliminarEmpresaCommand, Empresa>();

            CreateMap<Empresa, ObtenerModeloEmpresa>();
            CreateMap<Empresa, ModeloObtenerDetalleListaEmpresa>();
            #endregion

            #region Oficina
            CreateMap<CrearOficinaCommand, Oficina>()
                .ForMember(m => m.Empresa, opt => opt.MapFrom(d => new Empresa() { Id = d.IdEmpresa }));
            CreateMap<ModificarOficinaCommand, Oficina>()
                .ForMember(m => m.Empresa, opt => opt.MapFrom(d => new Empresa() { Id = d.IdEmpresa }));
            CreateMap<EliminarOficinaCommand, Oficina>();

            CreateMap<Oficina, ObtenerModeloOficina>()
                .ForMember(m => m.IdEmpresa, opt => opt.MapFrom(d => d.Empresa.Id))
                .ForMember(m => m.NombreEmpresa, opt => opt.MapFrom(d => d.Empresa.Nombre));
            CreateMap<Oficina, ModeloObtenerDetalleListaOficina>()
                .ForMember(m => m.IdEmpresa, opt => opt.MapFrom(d => d.Empresa.Id))
                .ForMember(m => m.NombreEmpresa, opt => opt.MapFrom(d => d.Empresa.Nombre));
            #endregion

            #region Log
            CreateMap<Log, ObtenerModeloLog>()
               .ForMember(m => m.IdOperacion, opt => opt.MapFrom(d => d.Operacion.Id))
               .ForMember(m => m.NombreOperacion, opt => opt.MapFrom(d => d.Operacion.Nombre))
               .ForMember(m => m.IdCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Id))
               .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido));
            CreateMap<Log, ModeloObtenerDetalleListaLog>()
               .ForMember(m => m.NombreOperacion, opt => opt.MapFrom(d => d.Operacion.Nombre))
               .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido));
            #endregion

            #region Tipo Catalogo
            CreateMap<ModificarTipoCatalogoCommand, TipoCatalogo>();
            #endregion


            CreateMap<LimiteExistencia, ModeloLimiteExistencia>().ReverseMap();
            CreateMap<LimiteTransaccional, ModeloLimiteTransaccional>().ReverseMap();

            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloEmpresa>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloOficina>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloPersona>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloSupervisor>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloCorresponsal>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloCatalogo>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloTipoCatalogo>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloDispositivo>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloLog>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloLimiteExistencia>>();
            CreateMap<ModeloPaginacion, ModeloFuenteDatos<ModeloLimiteTransaccional>>();
        }
    }
}
