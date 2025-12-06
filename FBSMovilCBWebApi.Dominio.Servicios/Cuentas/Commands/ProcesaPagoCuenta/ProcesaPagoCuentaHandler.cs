using AutoMapper;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class ProcesaPagoCuentaHandler : IRequestHandler<ProcesaPagoCuentaME, ProcesaPagoCuentasPorCobrarResponse>
    {
        private readonly ICuentasPorCobrarApi _cuentaPorCobrarApi;
        private readonly IHttpContextAccessor _httpContextAccesor;        
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioCuenta _repositorioCuenta;

        public ProcesaPagoCuentaHandler(
            ICuentasPorCobrarApi cuentaPorCobrarApi,             
            IMediator mediador, 
            IJsonConfiguracion jsonConfiguracion, 
            IHttpContextAccessor httpContextAccesor,
            IApiKeyGenerator apiKeyGenerator, 
            IRepositorioAgente repositorioAgente,
            IRepositorioCuenta repositorioCuenta)
        {
            _cuentaPorCobrarApi = cuentaPorCobrarApi;            
            _mediador = mediador;
            _jsonConfiguracion = jsonConfiguracion;
            _httpContextAccesor = httpContextAccesor;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
            _repositorioCuenta = repositorioCuenta;
        }

        public async Task<ProcesaPagoCuentasPorCobrarResponse> Handle(ProcesaPagoCuentaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCrearCuenta").Valor,
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

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCrearCuenta").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            var agente = await _repositorioAgente.GetForId(_httpContextAccesor.HttpContext.User.Identity.Name);
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());

            int secuencialCuenta = cuenta != null ? int.Parse(cuenta.SecuencialCuenta) : 0;

            var comision = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente).Deposito.Comisiones;
            var arregloComisiones = new List<ComisionFinancial>();
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Canal", ValorComision = comision.AdministracionCanal });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Agente", ValorComision = comision.Agente });
            arregloComisiones.Add(new ComisionFinancial() { NombreComision = "Cooperativa", ValorComision = comision.Cooperativa });

            var procesaPago = new ProcesaPagoCuentasPorCobrarRequest(
                request.IdentificacionCliente,
                "PagoCuentaPorCobrar",
                (List<RubroPorCobrarRequest>)request.CuentasPorCobrar,
                secuencialCuenta,
                request.ValorAfectado,
                JsonConvert.SerializeObject(arregloComisiones),
                request.Usuario
            );

            var respuesta = await _cuentaPorCobrarApi.ProcesaPagoCuentasPorCobrarAsync(procesaPago);           
            
            return respuesta;
        }
    }
}
