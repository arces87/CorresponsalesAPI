using AutoMapper;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteHandler : IRequestHandler<BuscarClienteME, InformacionPersonaMS>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;

        public BuscarClienteHandler(IFBSCorresponsalesApi financialApi, IMapper mapper)
        {
            _financialApi = financialApi;
            _mapper = mapper;
        }

        public async Task<InformacionPersonaMS> Handle(BuscarClienteME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.Clientes.DevuelveDatosPersonaIdentificacionWithHttpMessagesAsync(_mapper.Map<PorIdentificacionSocioME>(request));
            return respuesta.Body;
        }
    }
}
