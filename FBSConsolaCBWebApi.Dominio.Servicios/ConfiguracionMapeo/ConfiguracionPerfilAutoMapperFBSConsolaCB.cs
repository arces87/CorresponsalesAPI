using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.ConfiguracionMapeo;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Personas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands;
using Financial_Services_Banca.Models;

namespace FBSConsolaCBWebApi.Dominio.Servicios.ConfiguracionMapeo
{
    public class ConfiguracionPerfilAutoMapperFBSConsolaCB : ConfiguracionAutoMapper
    {
        public ConfiguracionPerfilAutoMapperFBSConsolaCB() : base()
        {

            #region Dispositivo
            CreateMap<CrearDispositivoCommand, Dispositivo>()
               .ForMember(m => m.TipoDispositivo, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdTipoDispositivo }));
            CreateMap<EliminarDispositivoCommand, Dispositivo>();
            CreateMap<Dispositivo, ObtenerModeloDispositivo>()
                .ForMember(m => m.IdTipoDispositivo, opt => opt.MapFrom(d => d.TipoDispositivo.Id))
                .ForMember(m => m.NombreTipoDispositivo, opt => opt.MapFrom(d => d.TipoDispositivo.Nombre));
            CreateMap<Dispositivo, ModeloObtenerDetalleListaDispositivo>()
                .ForMember(m => m.NombreTipoDispositivo, opt => opt.MapFrom(d => d.TipoDispositivo.Nombre));
            #endregion
            #region LimiteExistencia
            CreateMap<CrearLimiteExistenciaCommand, LimiteExistencia>()
               .ForMember(m => m.Corresponsal, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdCorresponsal }));
            CreateMap<EliminarLimiteExistenciaCommand, LimiteExistencia>();
            CreateMap<LimiteExistencia, ObtenerModeloLimiteExistencia>()
                .ForMember(m => m.IdCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Id))
                .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido));
            CreateMap<LimiteExistencia, ModeloObtenerDetalleListaLimiteExistencia>()
                .ForMember(m => m.IdCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Id))
                .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido));
            #endregion

            #region LimiteTransaccional
            CreateMap<CrearLimiteTransaccionalCommand, LimiteTransaccional>()
               .ForMember(m => m.Corresponsal, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdCorresponsal }))
               .ForMember(m => m.Operacion, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdOperacion }));
            CreateMap<EliminarLimiteTransaccionalCommand, LimiteTransaccional>();
            CreateMap<LimiteTransaccional, ObtenerModeloLimiteTransaccional>()
                .ForMember(m => m.IdCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Id))
                .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido))
                .ForMember(m => m.IdOperacion, opt => opt.MapFrom(d => d.Operacion.Id))
                .ForMember(m => m.NombreOperacion, opt => opt.MapFrom(d => d.Operacion.Nombre));
            CreateMap<LimiteTransaccional, ModeloObtenerDetalleListaLimiteTransaccional>()
                .ForMember(m => m.IdCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Id))
                .ForMember(m => m.NombreCorresponsal, opt => opt.MapFrom(d => d.Corresponsal.Persona.NombreUnido))
                .ForMember(m => m.IdOperacion, opt => opt.MapFrom(d => d.Operacion.Id))
                .ForMember(m => m.NombreOperacion, opt => opt.MapFrom(d => d.Operacion.Nombre));
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
                .ForMember(m => m.Remitente, opt => opt.MapFrom(d => new Persona() { Id = d.IdRemitente }))
                .ForMember(m => m.Categoria, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdCategoria }));
            CreateMap<EliminarAlertaCommand, Alerta>();

            CreateMap<Alerta, ObtenerModeloAlerta>()
                .ForMember(m => m.Destinatario, opt => opt.MapFrom(d => d.Destinatario.NombreUnido))
                .ForMember(m => m.Remitente, opt => opt.MapFrom(d => d.Remitente.NombreUnido))
                .ForMember(m => m.IdCategoria, opt => opt.MapFrom(d => d.Categoria.Id))
                .ForMember(m => m.NombreCategoria, opt => opt.MapFrom(d => d.Categoria.Nombre));
            CreateMap<Alerta, ModeloObtenerDetalleListaAlerta>()
                .ForMember(m => m.Destinatario, opt => opt.MapFrom(d => d.Destinatario.NombreUnido))
                .ForMember(m => m.Remitente, opt => opt.MapFrom(d => d.Remitente.NombreUnido));
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

            #region Persona
            CreateMap<EliminarPersonaCommand, Persona>();
            CreateMap<Persona, ModeloObtenerDetalleListaPersona>();
            CreateMap<Persona, ObtenerModeloPersona>()
                .ForMember(m => m.IdOficina, opt => opt.MapFrom(d => d.Oficina.Id))
                .ForMember(m => m.NombreOficina, opt => opt.MapFrom(d => d.Oficina.Nombre))
                .ForMember(m => m.IdTipoIdentificacion, opt => opt.MapFrom(d => d.TipoIdentificacion.Id))
                .ForMember(m => m.Usuario, opt => opt.MapFrom(d => d.Usuario.UserName));
            CreateMap<InformacionPersonaMS, ObtenerModeloPersonaIdentificacion>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Nombres.Split()[0]))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Nombres.Split().Length > 1 ? d.Nombres.Split()[1] : ""))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Apellidos.Split()[0]))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Apellidos.Split().Length > 1 ? d.Nombres.Split()[1] : ""))
                .ForMember(m => m.FechaNacimiento, opt => opt.MapFrom(d => d.FechaNacimientoCreacion));
            #endregion

            #region Corresponsales
            CreateMap<CrearRolCorresponsal, Rol>();
            CreateMap<CrearUsuarioCorresponsal, Usuario>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(d => d.Usuario))
                .ForMember(m => m.Email, opt => opt.MapFrom(d => d.CorreoElectronico))
                .ForMember(m => m.PasswordHash, opt => opt.MapFrom(d => d.Contrasenna));
            CreateMap<CrearPersonaCorresponsal, Persona>()
                .ForMember(m => m.Oficina, opt => opt.MapFrom(d => new Oficina() { Id = d.IdOficina }))
                .ForMember(m => m.TipoIdentificacion, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdTipoIdentificacion }));
            CreateMap<CrearCorresponsalCommand, Corresponsal>()
                .ForMember(m => m.Supervisor, opt => opt.MapFrom(d => new Supervisor() { Id = d.IdSupervisor }));
            CreateMap<ModificarRolCorresponsal, Rol>();
            CreateMap<ModificarUsuarioCorresponsal, Usuario>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(d => d.Usuario))
                .ForMember(m => m.Email, opt => opt.MapFrom(d => d.CorreoElectronico))
                .ForMember(m => m.PasswordHash, opt => opt.MapFrom(d => d.Contrasenna));
            CreateMap<ModificarPersonaCorresponsal, Persona>()
                .ForMember(m => m.Oficina, opt => opt.MapFrom(d => new Oficina() { Id = d.IdOficina }))
                .ForMember(m => m.TipoIdentificacion, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdTipoIdentificacion }));
            CreateMap<ModificarCorresponsalCommand, Corresponsal>()
                .ForMember(m => m.Supervisor, opt => opt.MapFrom(d => new Supervisor() { Id = d.IdSupervisor }));

            CreateMap<Corresponsal, ModeloObtenerDetalleListaCorresponsal>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Persona.PrimerNombre))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Persona.SegundoNombre))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Persona.PrimerApellido))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Persona.NombreUnido))
                .ForMember(m => m.NumeroIdentificador, opt => opt.MapFrom(d => d.Persona.NumeroIdentificador))
                .ForMember(m => m.Identificacion, opt => opt.MapFrom(d => d.Persona.Identificacion))
                .ForMember(m => m.IdOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Id))
                .ForMember(m => m.NombreOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Nombre))
                .ForMember(m => m.Estado, opt => opt.MapFrom(d => d.Persona.Usuario.LockoutEnabled ? "BLOQUEADO" : "ACTIVO"));

            CreateMap<Corresponsal, ObtenerModeloCorresponsal>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Persona.PrimerNombre))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Persona.SegundoNombre))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Persona.PrimerApellido))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Persona.NombreUnido))
                .ForMember(m => m.NumeroIdentificador, opt => opt.MapFrom(d => d.Persona.NumeroIdentificador))
                .ForMember(m => m.Identificacion, opt => opt.MapFrom(d => d.Persona.Identificacion))
                .ForMember(m => m.Direccion, opt => opt.MapFrom(d => d.Persona.Direccion))
                .ForMember(m => m.FechaNacimiento, opt => opt.MapFrom(d => d.Persona.FechaNacimiento))
                .ForMember(m => m.IdOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Id))
                .ForMember(m => m.NombreOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Nombre))
                .ForMember(m => m.IdTipoIdentificacion, opt => opt.MapFrom(d => d.Persona.TipoIdentificacion.Id))
                .ForMember(m => m.Identificacion, opt => opt.MapFrom(d => d.Persona.TipoIdentificacion.Nombre))
                .ForMember(m => m.IdSupervisor, opt => opt.MapFrom(d => d.Supervisor.Id))
                .ForMember(m => m.NombreSupervisor, opt => opt.MapFrom(d => d.Supervisor.Persona.NombreUnido))
                .ForMember(m => m.Estado, opt => opt.MapFrom(d => d.Persona.Usuario.LockoutEnabled ? "BLOQUEADO" : "ACTIVO"));

            #endregion

            #region Supervisores
            CreateMap<CrearRolSupervisor, Rol>();
            CreateMap<CrearUsuarioSupervisor, Usuario>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(d => d.Usuario))
                .ForMember(m => m.Email, opt => opt.MapFrom(d => d.CorreoElectronico))
                .ForMember(m => m.PasswordHash, opt => opt.MapFrom(d => d.Contrasenna));
            CreateMap<CrearPersonaSupervisor, Persona>()
                .ForMember(m => m.Oficina, opt => opt.MapFrom(d => new Oficina() { Id = d.IdOficina }))
                .ForMember(m => m.TipoIdentificacion, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdTipoIdentificacion }));
            CreateMap<CrearSupervisorCommand, Supervisor>();
            CreateMap<ModificarRolSupervisor, Rol>();
            CreateMap<ModificarUsuarioSupervisor, Usuario>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(d => d.Usuario))
                .ForMember(m => m.Email, opt => opt.MapFrom(d => d.CorreoElectronico))
                .ForMember(m => m.PasswordHash, opt => opt.MapFrom(d => d.Contrasenna));
            CreateMap<ModificarPersonaSupervisor, Persona>()
                .ForMember(m => m.Oficina, opt => opt.MapFrom(d => new Oficina() { Id = d.IdOficina }))
                .ForMember(m => m.TipoIdentificacion, opt => opt.MapFrom(d => new Catalogo() { Id = d.IdTipoIdentificacion }));
            CreateMap<ModificarSupervisorCommand, Supervisor>();

            CreateMap<Supervisor, ModeloObtenerDetalleListaSupervisor>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Persona.PrimerNombre))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Persona.SegundoNombre))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Persona.PrimerApellido))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Persona.NombreUnido))
                .ForMember(m => m.NumeroIdentificador, opt => opt.MapFrom(d => d.Persona.NumeroIdentificador))
                .ForMember(m => m.Identificacion, opt => opt.MapFrom(d => d.Persona.Identificacion))
                .ForMember(m => m.IdOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Id))
                .ForMember(m => m.NombreOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Nombre));
            CreateMap<Supervisor, ObtenerModeloSupervisor>()
                .ForMember(m => m.PrimerNombre, opt => opt.MapFrom(d => d.Persona.PrimerNombre))
                .ForMember(m => m.SegundoNombre, opt => opt.MapFrom(d => d.Persona.SegundoNombre))
                .ForMember(m => m.PrimerApellido, opt => opt.MapFrom(d => d.Persona.PrimerApellido))
                .ForMember(m => m.SegundoApellido, opt => opt.MapFrom(d => d.Persona.NombreUnido))
                .ForMember(m => m.NumeroIdentificador, opt => opt.MapFrom(d => d.Persona.NumeroIdentificador))
                .ForMember(m => m.Identificacion, opt => opt.MapFrom(d => d.Persona.Identificacion))
                .ForMember(m => m.Direccion, opt => opt.MapFrom(d => d.Persona.Direccion))
                .ForMember(m => m.FechaNacimiento, opt => opt.MapFrom(d => d.Persona.FechaNacimiento))
                .ForMember(m => m.IdOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Id))
                .ForMember(m => m.NombreOficina, opt => opt.MapFrom(d => d.Persona.Oficina.Nombre))
                .ForMember(m => m.IdTipoIdentificacion, opt => opt.MapFrom(d => d.Persona.TipoIdentificacion.Id))
                .ForMember(m => m.Usuario, opt => opt.MapFrom(d => d.Persona.Usuario.UserName))
                .ForMember(m => m.IdUsuario, opt => opt.MapFrom(d => d.Persona.Usuario.Id))
                .ForMember(m => m.CorreoElectronico, opt => opt.MapFrom(d => d.Persona.Usuario.Email))
                .ForMember(m => m.Identificacion, opt => opt.MapFrom(d => d.Persona.TipoIdentificacion.Nombre));

            #endregion

            #region Usuarios
            CreateMap<AutenticarUsuarioCommand, LoginUsuarioCommand>();
            CreateMap<ModeloUsuarioAutenticado, ModeloAutenticacion>();
            CreateMap<ModeloUsuarioRol, ModeloRolAutenticacion>();
            CreateMap<Persona, ModeloAutenticacion>()
                .ForMember(m => m.Usuario, opt => opt.Ignore())
                .ForMember(m => m.CorreoElectronico, opt => opt.Ignore())
                .ForMember(m => m.IdPersona, opt => opt.MapFrom(d => d.Id))
                .ForMember(m => m.IdOficina, opt => opt.MapFrom(d => d.Oficina.Id))
                .ForMember(m => m.NombreOficina, opt => opt.MapFrom(d => d.Oficina.Nombre))
                .ForMember(m => m.IdEmpresa, opt => opt.MapFrom(d => d.Oficina.Empresa.Id))
                .ForMember(m => m.NombreEmpresa, opt => opt.MapFrom(d => d.Oficina.Empresa.Nombre));

            #endregion

            #region Filtros
            CreateMap<ObtenerListaDispositivoQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaAlertaQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaCatalogoQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaCorresponsalQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaEmpresaQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaLimiteExistenciaQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaLimiteTransaccionalQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaLogQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaOficinaQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaPersonaQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaTipoCatalogoQuery, ModeloPaginacion>();
            CreateMap<ObtenerListaSupervisorQuery, ModeloPaginacion>();
            #endregion
        }
    }
}
