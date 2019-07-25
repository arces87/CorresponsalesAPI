using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LimiteExistenciaController : Controller
    {
        private readonly IMediator _mediador;

        public LimiteExistenciaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet]
        public async Task<ActionResult<ModeloObtenerLimiteExistencia>> List()
        {
            return await _mediador.Send(new ObtenerListaLimiteExistenciaQuery());
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ObtenerModeloLimiteExistencia>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerLimiteExistenciaQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearLimiteExistenciaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarLimiteExistenciaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarLimiteExistenciaCommand() { Id = Id });
        }
    }
}
