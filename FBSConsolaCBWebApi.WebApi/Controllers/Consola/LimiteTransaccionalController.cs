using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LimiteTransaccionalController : Controller
    {
        private readonly IMediator _mediador;

        public LimiteTransaccionalController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet]
        public async Task<ActionResult<ModeloObtenerLimiteTransaccional>> List()
        {
            return await _mediador.Send(new ObtenerListaLimiteTransaccionalQuery());
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ObtenerModeloLimiteTransaccional>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerLimiteTransaccionalQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearLimiteTransaccionalCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarLimiteTransaccionalCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarLimiteTransaccionalCommand() { Id = Id });
        }
    }
}
