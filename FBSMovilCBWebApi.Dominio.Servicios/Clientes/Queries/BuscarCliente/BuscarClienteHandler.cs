using AutoMapper;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
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
        private readonly IMediator _mediador;

        public BuscarClienteHandler(IFBSCorresponsalesApi financialApi, IMapper mapper, IMediator mediador)
        {
            _financialApi = financialApi;
            _mapper = mapper;
            _mediador = mediador;
        }

        public async Task<InformacionPersonaMS> Handle(BuscarClienteME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac =  request.Mac,
                Longitud =  request.Longitud,
                Latitud = request.Latitud
            });
            var respuesta = await _financialApi.Clientes.DevuelveDatosPersonaIdentificacionWithHttpMessagesAsync(_mapper.Map<PorIdentificacionSocioME>(request));
            return respuesta.Body;
        }
    }
}
