using FBS.Infraestructura.Interfaces;
using MediatR;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class ListarTiposIdentificacionHandler: IRequestHandler<ListarTiposIdentificacionME, TiposIdentificacionMSL>
    {
        private readonly IClientesApi _clienteApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ListarTiposIdentificacionHandler(IClientesApi clienteApi, IApiKeyGenerator apiKeyGenerator)
        {
            _clienteApi = clienteApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<TiposIdentificacionMSL> Handle(ListarTiposIdentificacionME request, CancellationToken cancellationToken)
        {
           
            var apiKey = _apiKeyGenerator.generateApiKey("000000000000000");
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            var respuesta = await _clienteApi.ClientesDevuelveTiposIdentificacionAsync();
            return respuesta;
        }
    }
}
