using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using ServiciosFinancial.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Facilito.Commands.ProcesarPago
{
    public class ProcesarPagoPostProcessor : IRequestPostProcessor<ProcesarPagoME, PagoFacilitoMSL>
    {
        private readonly IMediator _mediador;
        public ProcesarPagoPostProcessor(IMediator mediador)
        {
            _mediador = mediador;
        }
        public async Task Process(ProcesarPagoME request, PagoFacilitoMSL response, CancellationToken cancellationToken)
        {
            await _mediador.Publish(new NotificacionME
            {
                CorreoElectronicoDestinatario = "",
                NombreDestinatario = "",
                AsuntoCorreoElectronico = "",
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                Valores = new Dictionary<string, string>()
            });
        }
    }
}
