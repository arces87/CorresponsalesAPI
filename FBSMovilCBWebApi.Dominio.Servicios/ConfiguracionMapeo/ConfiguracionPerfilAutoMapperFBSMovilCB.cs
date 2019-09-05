using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
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
            CreateMap<CrearAlertaCommand, Alerta>();

            CreateMap<Alerta, ObtenerModeloAlerta>();
            CreateMap<Alerta, ModeloObtenerDetalleListaAlerta>();
            #endregion

            #region Log
            CreateMap<CrearLogCommand, Log>();
            CreateMap<Log, ObtenerModeloLog>();
            CreateMap<Log, ModeloObtenerDetalleListaLog>();
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
