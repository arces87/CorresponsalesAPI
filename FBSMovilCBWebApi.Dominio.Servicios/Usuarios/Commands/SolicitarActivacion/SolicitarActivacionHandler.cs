using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
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

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarActivacionHandler : IRequestHandler<SolicitarActivacionME, bool>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public SolicitarActivacionHandler(IMediator mediador, IRepositorioAgente repositorioAgente, IRepositorioDispositivo repositorioDispositivo, IMapper mapper,
            IRepositorioDispositivoAgente repositorioDispositivoAgente, IJsonConfiguracion jsonConfiguracion, IRepositorioGeolocalizacion repositorioGeolocalizacion)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _repositorioDispositivo = repositorioDispositivo;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
        }

        public async Task<bool> Handle(SolicitarActivacionME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdActivacion").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });
            var error = "";
            try
            {
                var agente = await _repositorioAgente.GetForUserName(request.Usuario);
                var dispositivoagente = await _repositorioDispositivoAgente.GetForAgente(agente.Id.ToString());
                var dispositivo = await _repositorioDispositivo.Get(dispositivoagente.DispositivoId.ToString());
                var idEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoUbicado").Valor;
                var idEstadoInactivo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoEliminado").Valor;

                if (agente != null) //Comprobacion de existencia del Agente
                {
                    if (agente.Estado.Id.ToString() == idEstadoInactivo)
                    {
                        throw new ExcepcionApp("Error en la validación de los datos de autenticación | A003");
                    }
                    else if (dispositivo != null && dispositivo.Imei.ToUpper() == request.Imei.ToUpper() && dispositivo.MacAddress.ToUpper() == request.Mac.ToUpper()) //Comprobación de existencia de dispositivo y sus datos
                    {
                        var _usuario = new LoginUsuarioME() { Usuario = request.Usuario, Contrasenna = request.Contrasenia, Dispositivo = "Movil" };
                        var usuarioAutenticado = await _mediador.Send(_usuario);
                        if (usuarioAutenticado.Errores == null)
                        {
                            agente.Estado = new Catalogo() { Id = new Guid(idEstado) };
                            await _repositorioAgente.UpdateEstado(agente);
                            var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(agente.Id.ToString());
                            if (geolocalizacion != null) //Guardando Geolocalización
                            {
                                await _repositorioGeolocalizacion.Remove(geolocalizacion);
                            }
                            await _repositorioGeolocalizacion.AdicionarGeolocalizacionAgente(request.Latitud, request.Longitud, agente.Id.ToString());
                            await _mediador.Send(new CrearLogME()
                            {
                                JsonLog = JsonConvert.SerializeObject(request),
                                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdActivacion").Valor,
                                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                            });
                            return true;
                        }
                        else
                        {
                            error = " | A003";
                        }
                        throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
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
            catch (Exception)
            {
                throw new ExcepcionApp("Error en la validación de los datos de autenticación" + error);
            }
        }
    }
}
