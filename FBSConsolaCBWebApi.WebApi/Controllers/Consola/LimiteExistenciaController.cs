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

        [HttpPost("lista", Name = "LimiteExistencia_ListarLimiteExistencia")]
        public async Task<ActionResult<ModeloObtenerLimiteExistencia>> List([FromBody] ObtenerListaLimiteExistenciaQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet("get/{id}", Name = "LimiteExistencia_ObtenerLimiteExistencia")]
        public async Task<ActionResult<ObtenerModeloLimiteExistencia>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerLimiteExistenciaQuery() { Id = Id });
        }

        [HttpPost(Name = "LimiteExistencia_CrearLimiteExistencia")]
        public async Task<ActionResult<int>> Create([FromBody] CrearLimiteExistenciaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "LimiteExistencia_ActualizarLimiteExistencia")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarLimiteExistenciaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}", Name = "LimiteExistencia_EliminarLimiteExistencia")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarLimiteExistenciaCommand() { Id = Id });
        }
    }
}
