using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
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

        [HttpPost("lista", Name = "Catalogo_ListarCatalogos")]
        public async Task<ActionResult<ListarCatalogoMS>> List([FromBody] ListarCatalogoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Catalogo_ObtenerCatalogo")]
        public async Task<ActionResult<ObtenerCatalogoMS>> Get([FromBody] ObtenerCatalogoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut("actualizar", Name = "Catalogo_ActualizarCatalogo")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarCatalogoME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("eliminar", Name = "Catalogo_EliminarCatalogo")]
        public async Task<ActionResult<int>> Delete([FromBody] EliminarCatalogoME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
