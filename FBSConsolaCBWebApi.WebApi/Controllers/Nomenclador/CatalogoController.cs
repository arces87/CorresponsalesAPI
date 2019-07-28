using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CatalogoController : Controller
    {
        private readonly IMediator _mediador;

        public CatalogoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Catalogo_ListarCatalogos")]
        public async Task<ActionResult<ModeloObtenerListaCatalogo>> List([FromBody] ObtenerListaCatalogoQuery model)
        {
            return await _mediador.Send(model);
        }

        [HttpGet("get/{id}", Name = "Catalogo_ObtenerCatalogo")]
        public async Task<ActionResult<ObtenerModeloCatalogo>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerCatalogoQuery() { Id = Id });
        }

        [HttpPost(Name = "Catalogo_CrearCatalogo")]
        public async Task<ActionResult<int>> Create([FromBody] CrearCatalogoCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPut(Name = "Catalogo_ActualizarCatalogo")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarCatalogoCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("{id}", Name = "Catalogo_EliminarCatalogo")]
        public async Task<ActionResult<int>> Delete(int Id)
        {
            return Ok(await _mediador.Send(new EliminarCatalogoCommand() { Id = Id }));
        }
    }
}
