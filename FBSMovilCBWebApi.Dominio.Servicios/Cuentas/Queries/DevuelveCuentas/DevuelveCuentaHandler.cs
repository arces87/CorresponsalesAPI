using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaHandler : IRequestHandler<DevuelveCuentaME, ConsolidadoCuentasMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;

        public DevuelveCuentaHandler(IFBSCorresponsalesApi financialApi, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _financialApi = financialApi;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
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

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var apyKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apyKey);

            var respuesta = await _financialApi.Cuentas.DevuelveConsolidadoCuentasWithHttpMessagesAsync(request, customHeaders);
            return respuesta.Body;
        }
    }
}
