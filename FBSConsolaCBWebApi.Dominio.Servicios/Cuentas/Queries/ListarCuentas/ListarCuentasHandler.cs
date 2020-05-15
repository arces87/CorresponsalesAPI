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

        public ListarCuentasHandler(IFBSCorresponsalesApi financialApi)
        {
            _financialApi = financialApi;
        }

        public async Task<ConsolidadoCuentasMSL> Handle(ListaCuentaME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.Cuentas.DevuelveConsolidadoCuentasIdentificacionWithHttpMessagesAsync(new PorIdentificacionClienteDeUnaEmpresa()
            {
                Identificacion = request.Identificacion,
                SecuencialTipoIdentificacion = request.TipoIdentificacion,
                SecuencialEmpresa = 1
            });
            return respuesta.Body;
        }
    }
}
