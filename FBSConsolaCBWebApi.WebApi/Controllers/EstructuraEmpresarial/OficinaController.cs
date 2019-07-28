using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class OficinaController : Controller
    {
        private readonly IMediator _mediador;

        public OficinaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Oficina_ListarOficinas")]
        public async Task<ActionResult<ModeloObtenerListaOficina>> Get(ObtenerListaOficinaQuery model)
        {
            return await _mediador.Send(model);
        }

        [HttpGet("{id}", Name = "Oficina_ObtenerOficina")]
        public async Task<ActionResult<ObtenerModeloOficina>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerOficinaQuery() { Id = Id });
        }

        [HttpPost(Name = "Oficina_CrearOficina")]
        public async Task<ActionResult<int>> Create([FromBody] CrearOficinaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "Oficina_ActualizarOficina")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarOficinaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}", Name = "Oficina_EliminarOficina")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarOficinaCommand() { Id = Id });
        }
    }
}
