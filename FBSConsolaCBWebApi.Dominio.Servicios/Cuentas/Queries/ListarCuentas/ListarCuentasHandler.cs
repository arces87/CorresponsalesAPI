using FBSConsolaCBWebApi.Infraestructura.Utiles;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using Org.OpenAPITools.Model;
using Org.OpenAPITools.Api;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListarCuentasHandler : IRequestHandler<ListaCuentaME, ConsolidadoCuentasMSL>
    {
        private readonly ICuentasApi _cuentaApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ListarCuentasHandler(ICuentasApi cuentaApi, IApiKeyGenerator apiKeyGenerator)
        {
            _cuentaApi = cuentaApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<ConsolidadoCuentasMSL> Handle(ListaCuentaME request, CancellationToken cancellationToken)
        {
           
            var apiKey = _apiKeyGenerator.generateApiKey("000000000000000");
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var respuesta = await _cuentaApi.CuentasDevuelveConsolidadoCuentasIdentificacionAsync(new PorIdentificacionClienteDeUnaEmpresaME()
            {
                Identificacion = request.Identificacion,
                SecuencialTipoIdentificacion = request.TipoIdentificacion,
                SecuencialEmpresa = 1
            });
            return respuesta;
        }
    }
}
