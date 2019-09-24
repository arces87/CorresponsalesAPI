using FBS.DAL.Nomenclador;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSMovilCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSMovilCB() : base()
        {
            #region Alerta
            CreateMap<CrearAlertaME, Alerta>()
               .ForMember(m => m.Estado, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdEstado) }))
               .ForMember(m => m.Agente, opt => opt.MapFrom(d => new Agente() { Id = new Guid(d.IdAgente) }))
               .ForMember(m => m.Tipo, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdEstado) }));
            CreateMap<Alerta, ModeloListaAlerta>()
                .ForMember(m => m.IdEstado, opt => opt.MapFrom(d => d.Estado.Id))
               .ForMember(m => m.NombreEstado, opt => opt.MapFrom(d => d.Estado.Nombre))
               .ForMember(m => m.IdAgente, opt => opt.MapFrom(d => d.Agente.Id))
               .ForMember(m => m.NombreAgente, opt => opt.MapFrom(d => d.Agente.NombreAgente))
               .ForMember(m => m.IdTipo, opt => opt.MapFrom(d => d.Tipo.Nombre))
               .ForMember(m => m.NombreTipo, opt => opt.MapFrom(d => d.Tipo.Nombre));
            #endregion

            #region Log
            CreateMap<CrearLogCommand, Log>();
            CreateMap<Log, ObtenerModeloLog>();
            CreateMap<Log, ModeloObtenerDetalleListaLog>();
            #endregion

            #region Usuario
            CreateMap<UsuarioME, LoginUsuarioME>()
                .ForMember(l => l.Contrasenna, opt => opt.MapFrom(p => p.Password))
                .ForMember(l => l.Usuario, opt => opt.MapFrom(p => p.UsuarioLogin));
            CreateMap<ModeloLoginUsuario, ProcesarLoginMS>();
            #endregion

        }
    }
}
