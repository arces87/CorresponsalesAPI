using AutoMapper;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ObtenerFormatosHandler : IRequestHandler<ObtenerFormatosME, FormatoMS>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;

        public ObtenerFormatosHandler(IFBSCorresponsalesApi facilitoApi, IMapper mapper)
        {
            _financialApi = facilitoApi;
            _mapper = mapper;
        }

        public async Task<FormatoMS> Handle(ObtenerFormatosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.PagoServiciosPagoAgil.FormatoWithHttpMessagesAsync(request);
            return respuesta.Body;
        }
    }
}
