using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class TransaccionController : Controller
    {
        private readonly IMediator _mediador;

        public TransaccionController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("procesarDeposito", Name = "Transaccion_ProcesarDeposito")]
        [Produces(typeof(RespuestaProcesoDepositoMS))]
        public async Task<ActionResult<RespuestaProcesoDepositoMS>> ProcesarDeposito([FromBody] ProcesarDepositoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesarRetiro", Name = "Transaccion_ProcesarRetiro")]
        [Produces(typeof(RespuestaProcesoRetiroMS))]
        public async Task<ActionResult<RespuestaProcesoRetiroMS>> ProcesarRetiro([FromBody] ProcesarRetiroME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
