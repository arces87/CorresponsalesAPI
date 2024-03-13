using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using Corresponsales.Query.Api;
using Corresponsales.Query.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion
{
    public class ListarTiposIdentificacionHandler: IRequestHandler<ListarTiposIdentificacionME, DevuelveTiposIdentificacionResponse>
    {
        private readonly IPersonaApi _personaApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        public ListarTiposIdentificacionHandler(IPersonaApi personaApi, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _personaApi = personaApi;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<DevuelveTiposIdentificacionResponse> Handle(ListarTiposIdentificacionME request, CancellationToken cancellationToken)
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
            //var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            //var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            var respuesta = await _personaApi.DevuelveTiposIdentificacionAsync();
            return respuesta;
        }
    }
}
