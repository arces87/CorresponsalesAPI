using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Agente.SolicitarSaldoCuenta
{
    public class SolicitarSaldoCuentaHandler : IRequestHandler<SolicitarSaldoCuentaME, double>
    {
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioTransaccionRetiro _repositorioTransaccionRetiro;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IHttpContextAccessor _httpContext;
        

        public SolicitarSaldoCuentaHandler(
            IMediator mediador,
            IJsonConfiguracion jsonConfiguracion,
            IFBSCorresponsalesApi financialApi,
            IRepositorioAgente repositorioAgente,
            IRepositorioTransaccionRetiro repositoriotransaccionRetiro,
            IRepositorioCuenta repositorioCuenta,
            IApiKeyGenerator apiKeyGenerator,
            IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _financialApi = financialApi;
            _repositorioAgente = repositorioAgente;
            _repositorioTransaccionRetiro = repositoriotransaccionRetiro;
            _repositorioCuenta = repositorioCuenta;
            _apiKeyGenerator = apiKeyGenerator;
            _httpContext = httpContext;            
        }

        public async Task<double> Handle(SolicitarSaldoCuentaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdSolicitarSaldoCuenta").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());
            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            DevuelveCuentaME cuentaAsociada = new DevuelveCuentaME() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) };
            var respuestaCuentaAsociada = await _financialApi.Cuentas.DevuelveCuentaWithHttpMessagesAsync(cuentaAsociada, customHeaders);
            return respuestaCuentaAsociada.Body.Saldo.Value;
        }
    }
}
