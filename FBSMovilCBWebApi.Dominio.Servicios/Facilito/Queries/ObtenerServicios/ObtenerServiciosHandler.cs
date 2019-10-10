using MediatR;
using ServiciosFacilito;
using ServiciosFacilito.Models;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerServiciosHandler : IRequestHandler<ObtenerServiciosME, ObtenerServiciosFacilitoResponse>
    {
        private readonly IFBSFacilitoAPI _facilitoApi;

        public ObtenerServiciosHandler(IFBSFacilitoAPI facilitoApi)
        {
            _facilitoApi = facilitoApi;
        }

        public async Task<ObtenerServiciosFacilitoResponse> Handle(ObtenerServiciosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _facilitoApi.ObtenerServiciosFacilitoWithHttpMessagesAsync();
            return respuesta.Body;
        }
    }
}
