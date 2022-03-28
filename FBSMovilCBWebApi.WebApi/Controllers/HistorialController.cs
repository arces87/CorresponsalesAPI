using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class HistorialController : Controller
    {
        private readonly IMediator _mediador;

        public HistorialController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("obtenerTransacciones", Name = "Historial_ObtenerTransacciones")]
        public async Task<ActionResult<ListarTransaccionesMS>> ObtenerTransacciones([FromBody] ListarTransaccionesME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
