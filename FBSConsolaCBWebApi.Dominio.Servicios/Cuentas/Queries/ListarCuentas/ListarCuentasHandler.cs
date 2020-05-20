using FBSConsolaCBWebApi.Infraestructura.Utiles;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListarCuentasHandler : IRequestHandler<ListaCuentaME, ConsolidadoCuentasMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ListarCuentasHandler(IFBSCorresponsalesApi financialApi, IApiKeyGenerator apiKeyGenerator)
        {
            _financialApi = financialApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<ConsolidadoCuentasMSL> Handle(ListaCuentaME request, CancellationToken cancellationToken)
        {
           
            var apyKey = _apiKeyGenerator.generateApiKey("000000000000000");
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apyKey);

            var respuesta = await _financialApi.Cuentas.DevuelveConsolidadoCuentasIdentificacionWithHttpMessagesAsync(new PorIdentificacionClienteDeUnaEmpresa()
            {
                Identificacion = request.Identificacion,
                SecuencialTipoIdentificacion = request.TipoIdentificacion,
                SecuencialEmpresa = 1
            }, customHeaders);
            return respuesta.Body;
        }
    }
}
