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
        public async Task<ActionResult<ModeloObtenerListaOficina>> Get([FromBody] ObtenerListaOficinaQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet(Name = "Oficina_ObtenerOficina")]
        public async Task<ActionResult<ObtenerModeloOficina>> Get(ObtenerOficinaQuery modelo)
        {
            return await _mediador.Send(modelo);
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

        [HttpDelete(Name = "Oficina_EliminarOficina")]
        public async Task<ActionResult<bool>> Delete(EliminarOficinaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
