using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using ServiciosFinancial.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarAbonoPrestamoPostProcessor : IRequestPostProcessor<ProcesarRetiroME, EfectivizacionPrestamoMS>
    {
        private readonly IMediator _mediador;
        public ProcesarAbonoPrestamoPostProcessor(IMediator mediador)
        {
            _mediador = mediador;
        }
        public async Task Process(ProcesarRetiroME request, EfectivizacionPrestamoMS response, CancellationToken cancellationToken)
        {
            await _mediador.Publish(new NotificacionME
            {
                Operacion = "AbonoPrestamo",
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
