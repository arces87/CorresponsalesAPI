using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerServiciosHandler : IRequestHandler<ObtenerServiciosME, ObtenerServiciosMS>
    {
        private readonly IFBSCorresponsalesApi _financialApi;

        public ObtenerServiciosHandler(IFBSCorresponsalesApi facilitoApi)
        {
            _financialApi = facilitoApi;
        }

        public async Task<ObtenerServiciosMS> Handle(ObtenerServiciosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.PagoServiciosFacilito.ObtenerServiciosWithHttpMessagesAsync();
            return respuesta.Body;
        }
    }
}
