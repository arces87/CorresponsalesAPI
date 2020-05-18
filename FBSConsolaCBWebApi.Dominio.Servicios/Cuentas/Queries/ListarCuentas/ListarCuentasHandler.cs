using FBSConsolaCBWebApi.Infraestructura.Utiles;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

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
            //var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var apiKey = _apiKeyGenerator.generateApiKey("00000000-0000000000");
            var customHeader = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var respuesta = await _financialApi.Cuentas.DevuelveConsolidadoCuentasIdentificacionWithHttpMessagesAsync(new PorIdentificacionClienteDeUnaEmpresa()
            {
                Identificacion = request.Identificacion,
                SecuencialTipoIdentificacion = request.TipoIdentificacion,
                SecuencialEmpresa = 1
            }, customHeader);
            return respuesta.Body;
        }
    }
}
