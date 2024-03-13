using FBS.DAL.Nomenclador;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands;
using System;
using Newtonsoft.Json.Linq;
using Corresponsales.Query.Model;
using Corresponsales.Command.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSMovilCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSMovilCB() : base()
        {
            #region Alerta
            CreateMap<CrearAlertaME, Alerta>()
               .ForMember(m => m.Tipo, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdTipo) }));
            CreateMap<Alerta, ModeloListaAlerta>()
                .ForMember(m => m.IdEstado, opt => opt.MapFrom(d => d.Estado.Id))
               .ForMember(m => m.NombreEstado, opt => opt.MapFrom(d => d.Estado.Nombre))
               .ForMember(m => m.IdAgente, opt => opt.MapFrom(d => d.Agente.Id))
               .ForMember(m => m.NombreAgente, opt => opt.MapFrom(d => d.Agente.NombreAgente))
               .ForMember(m => m.IdTipo, opt => opt.MapFrom(d => d.Tipo.Nombre))
               .ForMember(m => m.NombreTipo, opt => opt.MapFrom(d => d.Tipo.Nombre));
            #endregion
              
            #region Log
            CreateMap<CrearLogME, Log>()
               .ForMember(m => m.TipoAccion, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdTipoAccion) }))
               .ForMember(m => m.Estado, opt => opt.MapFrom(d => new Catalogo() { Id = new Guid(d.IdEstado) }));
            CreateMap<Log, ObtenerModeloLog>();
            CreateMap<Log, ModeloObtenerDetalleListaLog>();
            #endregion

            #region Usuario
            CreateMap<AutenticarUsuarioME, LoginUsuarioME>()
                .ForMember(l => l.Contrasenna, opt => opt.MapFrom(p => p.Contrasenia))
                .ForMember(l => l.Usuario, opt => opt.MapFrom(p => p.Usuario));
            CreateMap<ModeloLoginUsuario, AutenticarUsuarioMS>();
            CreateMap<ComisionOperacion, ComisionOperacionMS>();
            #endregion

            #region Distribuidos
            CreateMap<Catalogo, DistribuidoAlerta>();
            CreateMap<PaisResponse, DistribuidoPaises>();
            CreateMap<EstadoCivilResponse, DistribuidoEstadoCivil>();
            CreateMap<CreaClienteRequest, CrearClienteME> ();
            #endregion

            #region Transacciones
            CreateMap<Transaccion, ModeloTransaccion>()
                .ForMember(t => t.Tipo, opt => opt.MapFrom(m => m.Descripcion));
            CreateMap<Transaccion, ModeloListarHojaColecta>()
                .ForMember(t => t.Tipo, opt => opt.MapFrom(m => m.Descripcion))
                .ForMember(t => t.NumeroCuenta, opt => opt.MapFrom(m => JObject.Parse(m.JsonDatos)["NumeroCuentaCliente"]));
            #endregion

            #region Servicios Financial
            CreateMap<CrearCuentaME, CreaCuentaRequest>();
            CreateMap<CrearClienteME, CreaClienteRequest>();
            CreateMap<BuscarClienteME, DevuelveDatosPersonaIdentificacionRequest>();
            CreateMap<DevuelveTipoCuentaME, DevuelveConsolidadoCuentasRequest>();
            #endregion

            #region Pago Servicios
            CreateMap<DevuelveTiposIdentificacionResponse, DistribuidoTipoIdentificacion>();
            CreateMap<ComisionOperacion, ComisionPago>();

            #endregion
        }
    }
}
