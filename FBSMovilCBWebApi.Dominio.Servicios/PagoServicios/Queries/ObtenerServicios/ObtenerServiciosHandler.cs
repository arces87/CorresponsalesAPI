using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ObtenerServiciosHandler : IRequestHandler<ObtenerServiciosME, ServiciosMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;

        public ObtenerServiciosHandler(IFBSCorresponsalesApi facilitoApi)
        {
            _financialApi = facilitoApi;
        }

        public async Task<ServiciosMSL> Handle(ObtenerServiciosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.PagoServiciosPagoAgil.ServiciosWithHttpMessagesAsync();
            return respuesta.Body;
        }
    }
}
