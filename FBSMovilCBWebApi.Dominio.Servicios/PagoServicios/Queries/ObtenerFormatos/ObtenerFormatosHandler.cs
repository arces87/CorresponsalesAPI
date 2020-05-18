using AutoMapper;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
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
        private readonly IMediator _mediador;

        public ObtenerFormatosHandler(IFBSCorresponsalesApi facilitoApi, IMapper mapper, IMediator mediador)
        {
            _financialApi = facilitoApi;
            _mapper = mapper;
            _mediador = mediador;
        }

        public async Task<FormatoMS> Handle(ObtenerFormatosME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var respuesta = await _financialApi.PagoServiciosPagoAgil.FormatoWithHttpMessagesAsync(request);
            return respuesta.Body;
        }
    }
}
