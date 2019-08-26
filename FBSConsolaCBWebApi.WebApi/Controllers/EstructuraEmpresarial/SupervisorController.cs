using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SupervisorController : Controller
    {
        private readonly IMediator _mediador;

        public SupervisorController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Supervisor_ListarSupervisores")]
        public async Task<ActionResult<ModeloObtenerListaSupervisor>> ListaSupervisores([FromBody] ObtenerListaSupervisorQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Supervisor_ObtenerSupervisor")]
        public async Task<ActionResult<ObtenerModeloSupervisor>> Get([FromBody] ObtenerSupervisorQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Supervisor_CrearSupervisor")]
        public async Task<ActionResult<int>> Create([FromBody] CrearSupervisorCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "Supervisor_ActualizarSupervisor")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarSupervisorCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
