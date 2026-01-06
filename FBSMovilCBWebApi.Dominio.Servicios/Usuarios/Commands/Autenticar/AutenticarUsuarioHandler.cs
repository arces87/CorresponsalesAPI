using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using FBS.Dominio.Servicios.CorreoElectronico;
using System.Collections.Generic;
using System.IO;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioHandler : IRequestHandler<AutenticarUsuarioME, AutenticarUsuarioMS>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IConfiguracionCanal _configuracionCanal;
        private IdentityOptions _identityOptions;
        private readonly IGeneralesApi _envioSMSApi;

        public AutenticarUsuarioHandler(
            IMediator mediador,
            IRepositorioAgente repositorioAgente,
            IRepositorioDispositivoAgente repositorioDispositivoAgente, 
            IRepositorioDispositivo repositorioDispositivo,
            IMapper mapper,
            IJsonConfiguracion jsonConfiguracion,
            IRepositorioGeolocalizacion repositorioGeolocalizacion,
            IOptions<IdentityOptions> identityOptions,
            IConfiguracionCanal configuracionCanal,
            IGeneralesApi envioSMSApi)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _repositorioDispositivo = repositorioDispositivo;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _configuracionCanal = configuracionCanal;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
            _identityOptions = identityOptions.Value;
            _identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(_configuracionCanal.Negocio.TiempoBloqueo);
            _identityOptions.Lockout.MaxFailedAccessAttempts = _configuracionCanal.Negocio.NumeroMaximoIntentosFallidos;
            _envioSMSApi = envioSMSApi;
        }

        public async Task<AutenticarUsuarioMS> Handle(AutenticarUsuarioME request, CancellationToken cancellationToken)
        {
            var error = "";
            AutenticarUsuarioME requestCopia = new AutenticarUsuarioME { 
                Imei = request.Imei,
                Contrasenia = "",
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                Mac = request.Mac,
                Usuario = request.Usuario,
            };


            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(requestCopia),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAutenticacion").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });
            try
            {
                var agente = await _repositorioAgente.GetForUserName(request.Usuario);                

                if (agente != null) //Comprobacion de existencia del Agente y si se encuentra Activo
                {
                    var dispositivoagente = await _repositorioDispositivoAgente.GetForAgente(agente.Id.ToString());
                    var dispositivo = await _repositorioDispositivo.Get(dispositivoagente.DispositivoId.ToString());
                    var idEstadoActivo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoActivo").Valor;
                    var idEstadoCobrando = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoCobrando").Valor;
                    var estadoAcceso = "fallido";

                    var _usuario = _mapper.Map<LoginUsuarioME>(request);
                    _usuario.Dispositivo = "Movil";
                    var usuarioAutenticado = await _mediador.Send(_usuario);
                    if (dispositivo != null && dispositivo.EstaActivo && dispositivo.Imei.ToUpper() == request.Imei.ToUpper() && dispositivo.MacAddress.ToUpper() == request.Mac.ToUpper()) //Comprobación de existencia de dispositivo y sus datos
                    {                        
                        if (usuarioAutenticado.Errores == null || usuarioAutenticado.CambioContrasenia)
                        {
                            AutenticarUsuarioMS usuario = null;
                            if (usuarioAutenticado.CambioContrasenia)
                            {
                                usuario = new AutenticarUsuarioMS()
                                {
                                    Token = usuarioAutenticado.Token,
                                    CambioContrasenia = true,
                                    Estado = agente.Estado.Nombre
                                };
                                await _mediador.Send(new CrearLogME()
                                {
                                    JsonLog = JsonConvert.SerializeObject(usuario),
                                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAutenticacion").Valor,
                                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                                });
                                return usuario;
                            }
                            else if (agente.Estado.Id == new Guid(idEstadoActivo) || agente.Estado.Id == new Guid(idEstadoCobrando))
                            {
                                var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
                                usuario = new AutenticarUsuarioMS()
                                {
                                    Token = usuarioAutenticado.Token,
                                    Comisiones = new ComisionesMS(),
                                    Identificacion = agente.Identificacion,
                                    JsonNegocio = jsonNegocio,
                                    Estado = agente.Estado.Nombre,
                                    NombreMostrar = usuarioAutenticado.NombreMostrar,
                                    TelefonoCelular = usuarioAutenticado.TelefonoCelular,
                                    ReferenciaUbicacion = agente.Ubicacion,
                                    SecuencialTipoIdentificacion = agente.TipoIdentificacion
                                };

                                await _mediador.Send(new CrearLogME()
                                {
                                    JsonLog = JsonConvert.SerializeObject(usuario),
                                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAutenticacion").Valor,
                                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                                });

                                if (jsonNegocio != null)
                                {
                                    if (jsonNegocio.CobroServicios != null)
                                        usuario.Comisiones.CobroServicios = _mapper.Map<ComisionOperacionMS>(jsonNegocio.CobroServicios.Comisiones);
                                    if (jsonNegocio.Deposito != null)
                                        usuario.Comisiones.Deposito = _mapper.Map<ComisionOperacionMS>(jsonNegocio.Deposito.Comisiones);
                                    if (jsonNegocio.Retiro != null)
                                        usuario.Comisiones.Retiro = _mapper.Map<ComisionOperacionMS>(jsonNegocio.Retiro.Comisiones);
                                    if (jsonNegocio.AbonoPrestamos != null)
                                        usuario.Comisiones.AbonoPrestamos = _mapper.Map<ComisionOperacionMS>(jsonNegocio.AbonoPrestamos.Comisiones);
                                    if (jsonNegocio.Obligaciones != null)
                                        usuario.Comisiones.Obligaciones = _mapper.Map<ComisionOperacionMS>(jsonNegocio.Obligaciones.Comisiones);
                                }

                                if (jsonNegocio.VerificarGeolocalizacion)
                                {
                                    var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());
                                    if (geolocalizacion != null) //Comprobación de los datos de Geolocalización
                                    {
                                        var latitud_inicio = geolocalizacion.Latitud - 1;
                                        var latitud_fin = geolocalizacion.Latitud + 1;
                                        var longitud_inicio = geolocalizacion.Longitud - 1;
                                        var longitud_fin = geolocalizacion.Longitud + 1;
                                        if (request.Latitud >= latitud_inicio && request.Latitud <= latitud_fin && request.Longitud >= longitud_inicio && request.Longitud <= longitud_fin)
                                        {
                                            estadoAcceso = "exitoso";
                                            await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                                            return usuario;
                                        }
                                        else
                                        {
                                            error = " | A006";
                                            await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                                        }
                                    }
                                    else
                                    {
                                        error = " | A005";
                                        await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                                    }
                                }
                                else
                                {
                                    estadoAcceso = "exitoso";
                                    await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                                    return usuario;
                                }
                            }
                            else
                            {
                                error = " | A004";
                                await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                            }
                        }
                        else
                        {
                            //error = " | A003"; //usuarioAutenticado.Errores clave, usuario inactivo, intentos fallidos, ...
                            error = ProcesarMensajeError(usuarioAutenticado.Errores);
                            await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                        }
                    }
                    else
                    {
                        error = " | A002"; // Los datos del dispositivo no son correctos
                        await NotificarAcceso(usuarioAutenticado, agente, estadoAcceso, error);
                    }
                }
                else
                {
                    error = " | A001"; // No se encuentra registrado
                }

                throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
            }
            catch (Exception e)
            {
                throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
            }
        }

        private async Task NotificarAcceso(ModeloLoginUsuario usuario, FBSConsolaCBWebApi.DAL.Corresponsales.Agente agente, string estado, string error)
        {
            try
            {
                var plantillaCorreo = File.ReadAllText("Resources/EmailTemplate/notificacion_acceso.html");
                var fechaActual = DateTime.Now.ToString("dd/MM/yyyy H:mm");
                plantillaCorreo = plantillaCorreo.Replace("[:NOMBREUSUARIO:]", usuario.Usuario)
                        .Replace("[:ESTADO:]", estado)
                        .Replace("[:FECHA:]", fechaActual);

                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = "Notificación de Acceso",
                    Mensaje = plantillaCorreo,
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = usuario.CorreoElectronico,
                            Nombre = usuario.NombreMostrar
                        }
                    }
                });

                var plantillaSMS = File.ReadAllText("Resources/SmsTemplate/template_acceso.txt");
                plantillaSMS = plantillaSMS.Replace("[:ESTADO:]", estado)
                        .Replace("[:FECHA:]", fechaActual);

                var mensajeSMS = new EnvioSmsRequest()
                {
                    CodigoUsuarioCorresponsal = usuario.Usuario,
                    MensajeTexto = plantillaSMS,                 
                    NumeroIdentificacion = "",
                    SecuencialTipoIdentificacion = 0,
                    NumeroCelular = usuario.TelefonoCelular
                };                

                var respuesta = await _envioSMSApi.EnvioSmsAsync(mensajeSMS);
            }
            catch (Exception)
            {
                if(error != null && error != "")
                    throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
            }

        }

        private string ProcesarMensajeError(string error)
        {
            var mensaje = " | " + error.Substring(0, 4);
            return mensaje;
        }
    }
}
