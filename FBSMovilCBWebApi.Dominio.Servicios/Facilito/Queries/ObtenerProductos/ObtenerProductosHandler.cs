using AutoMapper;
using MediatR;
using ServiciosFacilito;
using ServiciosFacilito.Models;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerProductosHandler : IRequestHandler<ObtenerProductosME, ObtenerProductosFacilitoResponse>
    {
        private readonly IFBSFacilitoAPI _facilitoApi;
        private readonly IMapper _mapper;

        public ObtenerProductosHandler(IFBSFacilitoAPI facilitoApi, IMapper mapper)
        {
            _facilitoApi = facilitoApi;
            _mapper = mapper;
        }

        public async Task<ObtenerProductosFacilitoResponse> Handle(ObtenerProductosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _facilitoApi.ObtenerProductosFacilitoWithHttpMessagesAsync(_mapper.Map<DatosServicioFacilitoRequest>(request));
            return respuesta.Body;
        }
    }
}
