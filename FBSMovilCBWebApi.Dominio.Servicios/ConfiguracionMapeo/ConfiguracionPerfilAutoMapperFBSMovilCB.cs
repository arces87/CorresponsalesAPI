using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands;

namespace FBSMovilCBWebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSMovilCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSMovilCB() : base()
        {
            #region Alerta
            CreateMap<CrearAlertaCommand, Alerta>()
                .ForMember(m => m.Destinatario, opt => opt.MapFrom(d => new Persona() { Id = d.IdDestinatario }))
                .ForMember(m => m.Remitente, opt => opt.MapFrom(d => new Persona() { Id = d.IdRemitente }));

            CreateMap<Alerta, ObtenerModeloAlerta>();
            CreateMap<Alerta, ModeloObtenerDetalleListaAlerta>();
            #endregion

            #region Log
            CreateMap<CrearLogCommand, Log>()
                .ForMember(m => m.Operacion, opt => opt.MapFrom(d => new Persona() { Id = d.IdOperacion }))
                .ForMember(m => m.Corresponsal, opt => opt.MapFrom(d => new Persona() { Id = d.IdCorresponsal }));
            CreateMap<Log, ObtenerModeloLog>()
               .ForMember(m => m.IdOperacion, opt => opt.MapFrom(d => d.Operacion.Id))
               .ForMember(m => m.NombreOperacion, opt => opt.MapFrom(d => d.Operacion.Nombre))
               .ForMember(m => m.IdCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Id))
               .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido));
            CreateMap<Log, ModeloObtenerDetalleListaLog>()
               .ForMember(m => m.NombreOperacion, opt => opt.MapFrom(d => d.Operacion.Nombre))
               .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido));
            #endregion

            #region Usuario
            CreateMap<UsuarioME, LoginUsuarioCommand>()
                .ForMember(l => l.Contrasenna, opt => opt.MapFrom(p => p.Password))
                .ForMember(l => l.Usuario, opt => opt.MapFrom(p => p.UsuarioLogin));
            CreateMap<ModeloUsuarioAutenticado, ProcesarLoginMS>();
            #endregion

        }
    }
}
