using AutoMapper;
using MediatR;
using ServiciosFacilito;
using ServiciosFacilito.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ConsultaServiciosHandler : IRequestHandler<ConsultaServiciosME, ConsultaMS>
    {
        private readonly IFBSFacilitoAPI _facilitoApi;
        private readonly IMapper _mapper;

        public ConsultaServiciosHandler(IFBSFacilitoAPI facilitoApi, IMapper mapper)
        {
            _facilitoApi = facilitoApi;
            _mapper = mapper;
        }

        public async Task<ConsultaMS> Handle(ConsultaServiciosME request, CancellationToken cancellationToken)
        {
            var respuesta = await _facilitoApi.ServicioConsultaWithHttpMessagesAsync(_mapper.Map<ConsultaRequest>(request));
            return respuesta.Body;
        }
    }
}
