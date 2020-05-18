using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ObtenerServiciosHandler : IRequestHandler<ObtenerServiciosME, ServiciosMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMediator _mediador;

        public ObtenerServiciosHandler(IFBSCorresponsalesApi facilitoApi, IMediator mediador)
        {
            _financialApi = facilitoApi;
            _mediador = mediador;
        }

        public async Task<ServiciosMSL> Handle(ObtenerServiciosME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var respuesta = await _financialApi.PagoServiciosPagoAgil.ServiciosWithHttpMessagesAsync();
            return respuesta.Body;
        }
    }
}
