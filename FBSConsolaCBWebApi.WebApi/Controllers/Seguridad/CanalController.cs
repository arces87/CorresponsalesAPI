using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Canales.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Canales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CanalController : Controller
    {
        private readonly IMediator _mediador;

        public CanalController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("obtenerJsonNegocio", Name = "Canal_ObtenerJsonNegocio")]
        public async Task<ActionResult<JsonNegocioMS>> ObtenerJsonNegocio([FromBody] ObtenerJsonNegocioME modelo)
        {
            var usuario = await _mediador.Send(modelo);
            return usuario;
        }

        [HttpPost("lista", Name = "Canal_ListarCanals")]
        public async Task<ActionResult<ListarCanalMS>> List([FromBody] ListarCanalME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Canal_ObtenerCanal")]
        public async Task<ActionResult<ObtenerCanalMS>> Get([FromBody] ObtenerCanalME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut("actualizar", Name = "Canal_ActualizarCanal")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarCanalME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("eliminar", Name = "Canal_EliminarCanal")]
        public async Task<ActionResult<int>> Delete([FromBody] EliminarCanalME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
