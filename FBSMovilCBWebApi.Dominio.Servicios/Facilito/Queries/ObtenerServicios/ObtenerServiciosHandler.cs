using MediatR;
using ServiciosFacilito;
using ServiciosFacilito.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerServiciosHandler : IRequestHandler<ObtenerServiciosME, ObtenerServiciosFacilitoMS>
    {
        private readonly IFBSFacilitoAPI _facilitoApi;

        public ObtenerServiciosHandler(IFBSFacilitoAPI facilitoApi)
        {
            _facilitoApi = facilitoApi;
        }

        public async Task<ObtenerServiciosFacilitoMS> Handle(ObtenerServiciosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _facilitoApi.ObtenerServiciosFacilitoWithHttpMessagesAsync();
            return respuesta.Body;
        }
    }
}
