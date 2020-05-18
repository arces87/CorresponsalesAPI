using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaHandler : IRequestHandler<DevuelveCuentaME, ConsolidadoCuentasMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMediator _mediador;

        public DevuelveCuentaHandler(IFBSCorresponsalesApi financialApi, IMediator mediador)
        {
            _financialApi = financialApi;
            _mediador = mediador;
        }

        public async Task<ConsolidadoCuentasMSL> Handle(DevuelveCuentaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var respuesta = await _financialApi.Cuentas.DevuelveConsolidadoCuentasWithHttpMessagesAsync(request);
            return respuesta.Body;
        }
    }
}
