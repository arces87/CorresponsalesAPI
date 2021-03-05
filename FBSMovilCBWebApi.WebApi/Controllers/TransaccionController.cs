using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarDeposito;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class TransaccionController : Controller
    {
        private readonly IMediator _mediador;

        public TransaccionController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("procesarDeposito", Name = "Transaccion_ProcesarDeposito")]
        [Produces(typeof(AfectacionAUnCorresponsalDepositoMS))]
        public async Task<ActionResult<AfectacionAUnCorresponsalDepositoMS>> ProcesarDeposito([FromBody] ProcesarDepositoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesarRetiro", Name = "Transaccion_ProcesarRetiro")]
        [Produces(typeof(AfectacionAUnCorresponsalRepositorioMS))]
        public async Task<ActionResult<AfectacionAUnCorresponsalRepositorioMS>> ProcesarRetiro([FromBody] ProcesarRetiroME modelo)
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
