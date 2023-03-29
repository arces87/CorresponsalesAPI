using AutoMapper;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using Newtonsoft.Json;
using FBS.Identidad.DAL.Modelado;
using System.Linq;
using Org.OpenAPITools.Model;
using Org.OpenAPITools.Api;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteHandler : IRequestHandler<BuscarClienteME, InformacionPersonaMS>
    {
        private readonly IClientesApi _clienteApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public BuscarClienteHandler(
            IClientesApi clienteApi, 
            IMapper mapper, 
            IMediator mediador, 
            IApiKeyGenerator apiKeyGenerator, 
            IRepositorioAgente repositorioAgente,
            IJsonConfiguracion jsonConfiguracion)
        {
            _clienteApi = clienteApi;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
            _jsonConfiguracion = jsonConfiguracion;

        }

        public async Task<InformacionPersonaMS> Handle(BuscarClienteME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac =  request.Mac,
                Longitud =  request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });
            
            //var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            //var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            await _mediador.Send(new CrearLogME()
            {
                //JsonLog = JsonConvert.SerializeObject(customHeaders),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            var porIdentificacionSocioME = _mapper.Map<PorIdentificacionSocioME>(request);
            await _mediador.Send(new CrearLogME()
            {
                //JsonLog = JsonConvert.SerializeObject(porIdentificacionSocioME),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });
            var respuesta = await _clienteApi.ClientesDevuelveDatosPersonaIdentificacionAsync(porIdentificacionSocioME);
            return respuesta;
        }
    }
}
