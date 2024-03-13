using Corresponsales.Query.Api;
using Corresponsales.Query.Model;
using FBS.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class ListarTiposIdentificacionHandler: IRequestHandler<ListarTiposIdentificacionME, DevuelveTiposIdentificacionResponse>
    {
        private readonly IPersonaApi _clienteApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ListarTiposIdentificacionHandler(IPersonaApi clienteApi, IApiKeyGenerator apiKeyGenerator)
        {
            _clienteApi = clienteApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<DevuelveTiposIdentificacionResponse> Handle(ListarTiposIdentificacionME request, CancellationToken cancellationToken)
        {
           
            //var apiKey = _apiKeyGenerator.generateApiKey("000000000000000");
            //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            var respuesta = await _clienteApi.DevuelveTiposIdentificacionAsync();
            return respuesta;
        }
    }
}
