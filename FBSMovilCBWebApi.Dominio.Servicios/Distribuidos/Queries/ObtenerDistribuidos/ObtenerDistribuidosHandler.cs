using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Newtonsoft.Json;
using ServiciosFinancial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries
{
    public class ObtenerDistribuidosHandler : IRequestHandler<ObtenerDistribuidosME, ObtenerDistribuidosMS>
    {

        private readonly IRepositorioCatalogo _repositorioCatalogo;
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IMediator _mediador;

        public ObtenerDistribuidosHandler(IFBSCorresponsalesApi financialApi, IRepositorioCatalogo repositorioCatalogo, IJsonConfiguracion jsonConfiguracion, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _financialApi = financialApi;
            _repositorioCatalogo = repositorioCatalogo;
            _jsonConfiguracion = jsonConfiguracion;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
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
            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var catalogos = await _repositorioCatalogo.GetAllWithAssociations(true);
            var tiposAlertas = catalogos.Where(c => c.TipoCatalogo.Id == new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdTipoAlerta").Valor)).ToList();

            try
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(customHeaders),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });

                var respuestaTiposIdentificacion = await _financialApi.Clientes.DevuelveTiposIdentificacionWithHttpMessagesAsync(customHeaders);
                var respuestaPaisEstadoCivil = await _financialApi.Clientes.DevuelveDistribuidosWithHttpMessagesAsync(customHeaders);

                return new ObtenerDistribuidosMS() 
                { 
                    TiposIdentificaciones = _mapper.Map<IEnumerable<DistribuidoTipoIdentificacion>>(respuestaTiposIdentificacion.Body.TiposIdentificacion), 
                    TiposAlertas = _mapper.Map<IEnumerable<DistribuidoAlerta>>(tiposAlertas),
                    Paises = _mapper.Map<IEnumerable<DistribuidoPaises>>(respuestaPaisEstadoCivil.Body.Paises),
                    EstadoCivil = _mapper.Map<IEnumerable<DistribuidoEstadoCivil>>(respuestaPaisEstadoCivil.Body.EstadosCiviles)
                };
            }
            catch (Exception e)
            {
                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(e.Message),
                    IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
                });
                throw new ExcepcionApp("Ha ocurrido un error al obtener los tipos de identificación.");
            }
        }
    }
}
