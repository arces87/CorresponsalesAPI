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

        [HttpPost("lista", Name = "Corresponsal_ListarCorresponsal")]
        public async Task<ActionResult<ModeloObtenerListaCorresponsal>> ListaCorresponsales([FromBody] ObtenerListaCorresponsalQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Corresponsal_ObtenerCorresponsal")]
        public async Task<ActionResult<ObtenerModeloCorresponsal>> Get([FromBody] ObtenerCorresponsalQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Corresponsal_CrearCorresponsal")]
        public async Task<ActionResult<int>> Create([FromBody] CrearCorresponsalCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("activar", Name = "Corresponsal_ActivarCorresponsal")]
        public async Task<ActionResult> Activar([FromBody] ActivarCorresponsalCommand modelo)
        {
            await _mediador.Publish(modelo);
            return Ok();
        }

        [HttpPut(Name = "Corresponsal_ActualizarCorresponsal")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarCorresponsalCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
