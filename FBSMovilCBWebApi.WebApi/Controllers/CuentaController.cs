using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class CuentaController : Controller
    {
        private readonly IMediator _mediador;

        public CuentaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("crearCuenta", Name = "Cuenta_CrearCuenta")]
        [Produces(typeof(CreaCuentaMS))]
        public async Task<ActionResult<CreaCuentaMS>> CrearCuenta([FromBody] CrearCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarTipoCuenta", Name = "Cuenta_DevuelveTipoCuenta")]
        [Produces(typeof(TiposCuentaClienteMSL))]
        public async Task<ActionResult<TiposCuentaClienteMSL>> BuscarTipoCuenta([FromBody] DevuelveTipoCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("buscarCuentas", Name = "Cuenta_DevuelveCuentas")]
        [Produces(typeof(ConsolidadoCuentasMSL))]
        public async Task<ActionResult<ConsolidadoCuentasMSL>> BuscarCuenta([FromBody] DevuelveCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
