using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
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
        [Produces(typeof(AfectacionAUnCorresponsalMS))]
        public async Task<ActionResult<AfectacionAUnCorresponsalMS>> ProcesarDeposito([FromBody] ProcesarDepositoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesarRetiro", Name = "Transaccion_ProcesarRetiro")]
        [Produces(typeof(AfectacionAUnCorresponsalMS))]
        public async Task<ActionResult<AfectacionAUnCorresponsalMS>> ProcesarRetiro([FromBody] ProcesarRetiroME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaTipoTransaccion", Name = "Transaccion_ListarTiposTransacciones")]
        public async Task<ActionResult<ListarTipoTransaccionMS>> ListarTiposTransacciones([FromBody] ListarTipoTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
