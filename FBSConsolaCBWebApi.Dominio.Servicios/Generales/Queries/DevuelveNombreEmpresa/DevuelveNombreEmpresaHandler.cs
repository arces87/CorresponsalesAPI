using Corresponsales.Query.Api;
using Corresponsales.Query.Model;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBS.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Generales.Queries
{
    public class DevuelveNombreEmpresaHandler : IRequestHandler<DevuelveNombreEmpresaME, DevuelveNombreEmpresaResponse>
    {
        private readonly IGeneralesApi _generalesApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public DevuelveNombreEmpresaHandler(IGeneralesApi generalesApi, IApiKeyGenerator apiKeyGenerator)
        {
            _generalesApi = generalesApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<DevuelveNombreEmpresaResponse> Handle(DevuelveNombreEmpresaME request, CancellationToken cancellationToken)
        {
            var apiKey = _apiKeyGenerator.generateApiKey("000000000000000");
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var requestModel = new DevuelveNombreEmpresaRequest(request.Secuencial);
            var respuesta = await _generalesApi.DevuelveNombreEmpresaAsync(requestModel, cancellationToken: cancellationToken);

            return respuesta;
        }
    }
}

