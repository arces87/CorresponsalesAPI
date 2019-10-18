using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    [Authorize]
    public class TransaccionController : Controller
    {
        private readonly IMediator _mediador;

        public TransaccionController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("procesarDeposito", Name = "Transaccion_ProcesarDeposito")]
        [Produces(typeof(ProcesoDepositoMS))]
        public async Task<ActionResult<ProcesoDepositoMS>> ProcesarDeposito([FromBody] ProcesarDepositoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesarRetiro", Name = "Transaccion_ProcesarRetiro")]
        [Produces(typeof(ProcesoRetiroMS))]
        public async Task<ActionResult<ProcesoRetiroMS>> ProcesarRetiro([FromBody] ProcesarRetiroME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
