using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBS.Infraestructura.Excepciones;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesaReimprimirTransaccionHandler : IRequestHandler<ProcesaReimprimirTransaccionME, ProcesaReimpresionPagoServicioResponse>
    {
        private readonly IMediator _mediador;
        private readonly IPagoApi _pagoApi;
        private readonly IHttpContextAccessor _httpContext;

        public ProcesaReimprimirTransaccionHandler(
            IMediator mediador,
            IPagoApi pagoApi,
            IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _pagoApi = pagoApi;
            _httpContext = httpContext;
        }

        public async Task<ProcesaReimpresionPagoServicioResponse> Handle(ProcesaReimprimirTransaccionME request, CancellationToken cancellationToken)
        {
            // Verificar agente
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            // Logging
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request.Request),
                IdTipoAccion = "ReimprimirTransaccion",
                IdEstado = "Solicitado"
            });

            // Llamar al API
            var response = await _pagoApi.ProcesaReimprimirTransaccionAsync(request.Request);

            // Logging
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(response),
                IdTipoAccion = "ReimprimirTransaccion",
                IdEstado = "Recibido"
            });

            return response;
        }
    }
}