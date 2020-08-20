using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HojaColectaController : Controller
    {
        private readonly IMediator _mediador;

        public HojaColectaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("cerrarDia", Name = "HojaColecta_CerrarDia")]
        public async Task<ActionResult<bool>> CerrarDia([FromBody] CerrarDiaME modelo)
        {
            await _mediador.Send(modelo);
            return true;
        }

        [HttpPost("obtenerTransacciones", Name = "HojaColecta_ObtenerTransacciones")]
        public async Task<ActionResult<ListarHojaColectaMS>> ObtenerTransacciones([FromBody] ListarHojaColectaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
