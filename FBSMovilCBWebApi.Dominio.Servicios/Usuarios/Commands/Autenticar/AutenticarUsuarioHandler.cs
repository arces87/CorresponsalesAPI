using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioHandler : IRequestHandler<AutenticarUsuarioME, AutenticarUsuarioMS>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public AutenticarUsuarioHandler(IMediator mediador, IRepositorioAgente repositorioAgente, IMapper mapper,
            IJsonConfiguracion jsonConfiguracion, IRepositorioGeolocalizacion repositorioGeolocalizacion)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
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
                var idEstadoActivo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoActivo").Valor;
                var idEstadoCobrando = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoCobrando").Valor;
                if (agente != null) //Comprobacion de existencia del Agente y si se encuentra Activo
                {
                    if (agente.Dispositivo != null && agente.Dispositivo.Imei.ToUpper() == request.Imei.ToUpper() && agente.Dispositivo.MacAddress.ToUpper() == request.Mac.ToUpper()) //Comprobación de existencia de dispositivo y sus datos
                    {
                        var _usuario = _mapper.Map<LoginUsuarioME>(request);
                        _usuario.Dispositivo = "Movil";
                        var usuarioAutenticado = await _mediador.Send(_usuario);
                        if (usuarioAutenticado.Errores == null || usuarioAutenticado.CambioContrasenia)
                        {
                            AutenticarUsuarioMS usuario = null;
                            if (usuarioAutenticado.CambioContrasenia)
                            {
                                usuario = new AutenticarUsuarioMS()
                                {
                                    Token = usuarioAutenticado.Token,
                                    CambioContrasenia = true
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
                                    Estado = agente.Estado.Nombre
                                };
                                if (jsonNegocio != null)
                                {
                                    if (jsonNegocio.CobroServicios != null)
                                        usuario.Comisiones.CobroServicios = _mapper.Map<ComisionOperacionMS>(jsonNegocio.CobroServicios.Comisiones);
                                    if (jsonNegocio.Deposito != null)
                                        usuario.Comisiones.Deposito = _mapper.Map<ComisionOperacionMS>(jsonNegocio.Deposito.Comisiones);
                                    if (jsonNegocio.Retiro != null)
                                        usuario.Comisiones.Retiro = _mapper.Map<ComisionOperacionMS>(jsonNegocio.Retiro.Comisiones);
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

                                            await _mediador.Send(new CrearLogME()
                                            {
                                                JsonLog = JsonConvert.SerializeObject(usuario),
                                                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAutenticacion").Valor,
                                                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                                            });
                                            return usuario;

                                        }
                                        else
                                        {
                                            error = " | A006";
                                        }
                                    }
                                    else
                                    {
                                        error = " | A005";
                                    }
                                }
                                else
                                {
                                    return usuario;
                                }
                            }
                            else
                            {
                                error = " | A004";
                            }
                        }
                        else
                        {
                            error = " | A003";
                        }
                    }
                    else
                    {
                        error = " | A002";
                    }
                }
                else
                {
                    error = " | A001";
                }

                throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
            }
            catch (Exception e)
            {
                throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
            }
        }
    }
}
