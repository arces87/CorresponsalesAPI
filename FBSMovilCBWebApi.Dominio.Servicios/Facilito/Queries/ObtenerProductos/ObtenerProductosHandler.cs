using AutoMapper;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerProductosHandler : IRequestHandler<ObtenerProductosME, ObtenerProductosMS>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;

        public ObtenerProductosHandler(IFBSCorresponsalesApi facilitoApi, IMapper mapper)
        {
            _financialApi = facilitoApi;
            _mapper = mapper;
        }

        public async Task<ObtenerProductosMS> Handle(ObtenerProductosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.PagoServiciosFacilito.ObtenerProductosWithHttpMessagesAsync(_mapper.Map<ServiciosFinancial.Models.ObtenerProductosME>(request));
            return respuesta.Body;
        }
    }
}
