using AutoMapper;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
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
            var respuesta = await _financialApi.PagoServiciosPagoAgil.ConsultaAsync(_mapper.Map<ConsultaME>(request));
            return respuesta;
        }
    }
}
