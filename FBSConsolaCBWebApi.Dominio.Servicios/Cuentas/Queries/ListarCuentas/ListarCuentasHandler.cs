using FBSConsolaCBWebApi.Infraestructura.Utiles;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using Corresponsales.Query.Model;
using Corresponsales.Query.Api;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListarCuentasHandler : IRequestHandler<ListaCuentaME, DevuelveConsolidadoCuentasIdentificacionResponse>
    {
        private readonly ICaptacionesVistaApi _cuentaApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ListarCuentasHandler(ICaptacionesVistaApi cuentaApi, IApiKeyGenerator apiKeyGenerator)
        {
            _cuentaApi = cuentaApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<DevuelveConsolidadoCuentasIdentificacionResponse> Handle(ListaCuentaME request, CancellationToken cancellationToken)
        {
           
            var apiKey = _apiKeyGenerator.generateApiKey("000000000000000");
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var respuesta = await _cuentaApi.DevuelveConsolidadoCuentasIdentificacionAsync(new DevuelveConsolidadoCuentasIdentificacionRequest()
            {
                Identificacion = request.Identificacion,
                SecuencialTipoIdentificacion = request.TipoIdentificacion,
                SecuencialEmpresa = 1
            });
            return respuesta;
        }
    }
}
