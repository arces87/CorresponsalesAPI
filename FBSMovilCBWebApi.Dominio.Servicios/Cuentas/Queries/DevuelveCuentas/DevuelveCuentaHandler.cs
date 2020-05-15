using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaHandler : IRequestHandler<DevuelveCuentaME, ConsolidadoCuentasMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;

        public DevuelveCuentaHandler(IFBSCorresponsalesApi financialApi)
        {
            _financialApi = financialApi;
        }

        public async Task<ConsolidadoCuentasMSL> Handle(DevuelveCuentaME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.Cuentas.DevuelveConsolidadoCuentasWithHttpMessagesAsync(request);
            return respuesta.Body;
        }
    }
}
