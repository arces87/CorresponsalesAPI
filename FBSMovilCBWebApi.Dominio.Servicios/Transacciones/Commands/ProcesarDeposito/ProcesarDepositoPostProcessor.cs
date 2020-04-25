using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using ServiciosFinancial.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarDepositoPostProcessor : IRequestPostProcessor<ProcesarDepositoME, AfectacionAUnCorresponsalMS>
    {
        private readonly IMediator _mediador;
        public ProcesarDepositoPostProcessor(IMediator mediador)
        {
            _mediador = mediador;
        }
        public async Task Process(ProcesarDepositoME request, AfectacionAUnCorresponsalMS response, CancellationToken cancellationToken)
        {
            await _mediador.Publish(new NotificacionME
            {
                Operacion = "Deposito",
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
