using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Commands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TransaccionController : Controller
    {
        private readonly IMediator _mediador;

        public TransaccionController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Transaccion_ListarTransacciones")]
        public async Task<ActionResult<ListarTransaccionMS>> List([FromBody] ListarTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaAgenteTipoTransaccion", Name = "Transaccion_ListarTransaccionesPorAgenteTipo")]
        public async Task<ActionResult<ListarTransaccionAgenteMS>> ListarPorAgenteTipo([FromBody] ListarTransaccionAgenteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaTipoTransaccion", Name = "Transaccion_ListarTiposTransacciones")]
        public async Task<ActionResult<ListarTipoTransaccionMS>> ListarTiposTransacciones([FromBody] ListarTipoTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("obtenerComisiones", Name = "Transaccion_ObtenerComisiones")]
        public async Task<ActionResult<ObtenerComisionTransaccionMS>> ObtenerComisiones([FromBody] ObtenerComisionTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("obtenerComisionesPorTipo", Name = "Transaccion_ObtenerComisionesPorTipo")]
        public async Task<ActionResult<ObtenerComisionPorTipoTransaccionMS>> ObtenerComisionesPorTipo([FromBody] ObtenerComisionPorTipoTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtenerRecaudaciones", Name = "Transaccion_ObtenerRecaudaciones")]
        public async Task<ActionResult<ListarRecaudacionesMS>> ObtenerRecaudaciones([FromBody] ListarRecaudacionesME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Transaccion_ObtenerTransaccion")]
        public async Task<ActionResult<ObtenerTransaccionMS>> Get([FromBody] ObtenerTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("reponerTransacciones", Name = "Transaccion_ReponerTransacciones")]
        public async Task<ActionResult<bool>> ReponerTransacciones([FromBody] ReponerTransaccionesME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
