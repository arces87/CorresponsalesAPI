using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CorresponsalController : Controller
    {
        private readonly IMediator _mediador;

        public CorresponsalController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloObtenerListaCorresponsal>> ListaCorresponsales([FromBody] ModeloPaginacion filtro)
        {
            return await _mediador.Send(new ObtenerListaCorresponsalQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ObtenerModeloCorresponsal>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerCorresponsalQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearCorresponsalCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("activar")]
        public async Task<ActionResult> Activar([FromBody] ActivarCorresponsalCommand modelo)
        {
            await _mediador.Publish(modelo);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarCorresponsalCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
