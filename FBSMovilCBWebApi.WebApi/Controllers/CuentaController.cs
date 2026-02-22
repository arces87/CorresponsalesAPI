using System.Threading.Tasks;
using Corresponsales.Command.Model;
using Corresponsales.Query.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Agente.SolicitarSaldoCuenta;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class CuentaController : Controller
    {
        private readonly IMediator _mediador;

        public CuentaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("crearCuenta", Name = "Cuenta_CrearCuenta")]
        [Produces(typeof(CreaCuentaResponse))]
        public async Task<ActionResult<CreaCuentaResponse>> CrearCuenta([FromBody] CrearCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarTipoCuenta", Name = "Cuenta_DevuelveTipoCuenta")]
        [Produces(typeof(DevuelveTiposDeCuentasDeUnClienteResponse))]
        public async Task<ActionResult<DevuelveTiposDeCuentasDeUnClienteResponse>> BuscarTipoCuenta([FromBody] DevuelveTipoCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarCuentas", Name = "Cuenta_DevuelveCuentas")]
        [Produces(typeof(DevuelveConsolidadoCuentasResponse))]
        public async Task<ActionResult<DevuelveConsolidadoCuentasResponse>> BuscarCuenta([FromBody] FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries.DevuelveCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("solicitudSaldoCuenta", Name = "Cuenta_SolicitarSaldoCuenta")]
        public async Task<ActionResult<double>> SolicitarSaldoCuenta([FromBody] SolicitarSaldoCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("cuentasPorCobrar", Name = "Cuenta_CuentasPorCobrar")]
        [Produces(typeof(DevuelveCuentasPorCobrarDeUnClienteResponse))]
        public async Task<ActionResult<DevuelveCuentasPorCobrarDeUnClienteResponse>> DevuelveCuentasPorCobrar([FromBody] DevuelveCuentasPorCobrarME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesaCuentasPorCobrar", Name = "Cuenta_ProcesaCuentasPorCobrar")]
        [Produces(typeof(ProcesaPagoCuentaMS))]
        public async Task<ActionResult<ProcesaPagoCuentaMS>> ProcesaPagoCuenta([FromBody] ProcesaPagoCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("aperturaCuenta", Name = "Cuenta_AperturaCuenta")]
        [Produces(typeof(AperturaCuentaResponse))]
        public async Task<ActionResult<AperturaCuentaResponse>> AperturaCuenta([FromBody] FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands.AperturaCuenta.AperturaCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
