using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class DevuelveSimulacionPagoHandler : IRequestHandler<DevuelveSimulacionPagoME, DevuelveSimulacionPagoResponse>
    {
        private readonly IMediator _mediador;
        private readonly IPagoApi _pagoApi;
        private readonly IHttpContextAccessor _httpContext;

        public DevuelveSimulacionPagoHandler(
            IMediator mediador,
            IPagoApi pagoApi,
            IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _pagoApi = pagoApi;
            _httpContext = httpContext;
        }

        public async Task<DevuelveSimulacionPagoResponse> Handle(DevuelveSimulacionPagoME request, CancellationToken cancellationToken)
        {
            // Verificar agente si es necesario
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            // Logging de la solicitud
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request.Request),
                IdTipoAccion = "SimulacionPago", // Ajustar según parametrización
                IdEstado = "Solicitado" // Ajustar según parametrización
            });

            // Llamar al API
            var response = await _pagoApi.DevuelveSimulacionPagoAsync(request.Request);

            // Logging de la respuesta
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(response),
                IdTipoAccion = "SimulacionPago",
                IdEstado = "Recibido"
            });

            return response;
        }
    }
}