using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Org.OpenAPITools.Api;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;

namespace FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries
{
    public class ObtenerDistribuidosHandler : IRequestHandler<ObtenerDistribuidosME, ObtenerDistribuidosMS>
    {

        private readonly IRepositorioCatalogo _repositorioCatalogo;
        private readonly IClientesApi _clienteApi;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IMediator _mediador;

        public ObtenerDistribuidosHandler(IClientesApi clienteApi, IRepositorioCatalogo repositorioCatalogo, IJsonConfiguracion jsonConfiguracion, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente, IRepositorioDispositivo repositorioDispositivo, IRepositorioDispositivoAgente repositorioDispositivoAgente)
        {
            _clienteApi = clienteApi;
            _repositorioCatalogo = repositorioCatalogo;
            _jsonConfiguracion = jsonConfiguracion;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
            _repositorioDispositivo = repositorioDispositivo;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
        }

        public async Task<ObtenerDistribuidosMS> Handle(ObtenerDistribuidosME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            //var dispositivoagente = await _repositorioDispositivoAgente.GetForAgente(agente.Id.ToString());
            //var dispositivo = await _repositorioDispositivo.Get(dispositivoagente.DispositivoId.ToString());
            //var apiKey = _apiKeyGenerator.generateApiKey(dispositivo.Imei);
            //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var catalogos = await _repositorioCatalogo.GetAllWithAssociations(true);
            var tiposAlertas = catalogos.Where(c => c.TipoCatalogo.Id == new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTipoAlerta").Valor)).ToList();
            var respuesta = new ObtenerDistribuidosMS() { 
                TiposAlertas = _mapper.Map<IEnumerable<DistribuidoAlerta>>(tiposAlertas)
            };                  

            await RecuperarTiposIdentificacion(request, respuesta);
            await RecuperarDistribuidos(respuesta);
            return respuesta;
        }

        private async Task RecuperarDistribuidos(ObtenerDistribuidosMS respuesta)
        {
            try
            {
                var respuestaPaisEstadoCivil = await _clienteApi.ClientesDevuelveDistribuidosAsync();
                respuesta.Paises = _mapper.Map<IEnumerable<DistribuidoPaises>>(respuestaPaisEstadoCivil.Paises);
                respuesta.EstadoCivil = _mapper.Map<IEnumerable<DistribuidoEstadoCivil>>(respuestaPaisEstadoCivil.EstadosCiviles);
            }
            catch (Exception e)
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(e.Message),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
                throw new ExcepcionApp("Ha ocurrido un error al obtener los distribuidos.");
            }
        }

        private async Task RecuperarTiposIdentificacion(ObtenerDistribuidosME request, ObtenerDistribuidosMS respuesta)
        {
            try
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(request),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });

                var respuestaTiposIdentificacion = await _clienteApi.ClientesDevuelveTiposIdentificacionAsync();
                respuesta.TiposIdentificaciones = _mapper.Map<IEnumerable<DistribuidoTipoIdentificacion>>(respuestaTiposIdentificacion.TiposIdentificacion);
            }
            catch (Exception e)
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(e.Message),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
                throw new ExcepcionApp("Ha ocurrido un error al obtener los distribuidos.");
            }
        }
    }
}
