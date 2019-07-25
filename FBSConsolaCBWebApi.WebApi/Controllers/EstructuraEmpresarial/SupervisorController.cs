using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
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

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloObtenerListaSupervisor>> ListaSupervisores([FromBody] ModeloPaginacion filtro)
        {
            return await _mediador.Send(new ObtenerListaSupervisorQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ObtenerModeloSupervisor>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerSupervisorQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearSupervisorCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarSupervisorCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
