using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        [Produces(typeof(CrearCuentaMS))]
        public async Task<ActionResult<CrearCuentaMS>> CrearCuenta([FromBody] CrearCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarTipoCuenta", Name = "Cuenta_DevuelveTipoCuenta")]
        [Produces(typeof(DevuelveTipoCuentaMS))]
        public async Task<ActionResult<DevuelveTipoCuentaMS>> BuscarTipoCuenta([FromBody] DevuelveTipoCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("buscarCuenta", Name = "Cuenta_DevuelveCuenta")]
        [Produces(typeof(DevuelveCuentaMS))]
        public async Task<ActionResult<DevuelveCuentaMS>> BuscarCuenta([FromBody] DevuelveCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
