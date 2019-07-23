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

        [HttpGet("{tipo}")]
        public async Task<ActionResult<ModeloObtenerListaCatalogo>> List(int Tipo)
        {
            return await _mediador.Send(new ObtenerListaCatalogoQuery());
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ObtenerModeloCatalogo>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerCatalogoQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearCatalogoCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarCatalogoCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int Id)
        {
            return Ok(await _mediador.Send(new EliminarCatalogoCommand() { Id = Id }));
        }
    }
}
