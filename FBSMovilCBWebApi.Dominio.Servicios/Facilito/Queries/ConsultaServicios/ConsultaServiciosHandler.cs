using AutoMapper;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ConsultaServiciosHandler : IRequestHandler<ConsultaServiciosME, ConsultaMS>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;

        public ConsultaServiciosHandler(IFBSCorresponsalesApi facilitoApi, IMapper mapper)
        {
            _financialApi = facilitoApi;
            _mapper = mapper;
        }

        public async Task<ConsultaMS> Handle(ConsultaServiciosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.PagoServiciosFacilito.ConsultaValorAPagarWithHttpMessagesAsync(_mapper.Map<ConsultaME>(request));
            return respuesta.Body;
        }
    }
}
