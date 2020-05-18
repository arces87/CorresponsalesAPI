using AutoMapper;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
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
        private readonly IMediator _mediador;

        public ConsultaServiciosHandler(IFBSCorresponsalesApi facilitoApi, IMapper mapper, IMediator mediador)
        {
            _financialApi = facilitoApi;
            _mapper = mapper;
            _mediador = mediador;
        }

        public async Task<ConsultaMS> Handle(ConsultaServiciosME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var respuesta = await _financialApi.PagoServiciosPagoAgil.ConsultaAsync(_mapper.Map<ConsultaME>(request));
            return respuesta;
        }
    }
}
