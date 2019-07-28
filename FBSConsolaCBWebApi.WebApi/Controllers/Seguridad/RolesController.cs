using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Roles.Commands;
using FBS.Identidad.Dominio.Servicios.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediador;

        public RoleController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet(Name = "Rol_ListarRoles")]
        public async Task<ActionResult<ModeloObtenerListaRol>> Roles()
        {
            return await _mediador.Send(new ObtenerListaRolQuery());
        }

        [HttpGet("{Id}", Name = "Rol_ObtenerRol")]
        public async Task<ActionResult<ObtenerModeloRol>> GetRole(string Id)
        {
            return await _mediador.Send(new ObtenerRolQuery() { Id = Id });
        }

        [HttpPost(Name = "Rol_CrearRol")]
        public async Task<ActionResult<int>> Create([FromBody] CrearRolCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "Rol_ActualizarRol")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarRolCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}", Name = "Rol_EliminarRol")]
        public async Task<ActionResult<bool>> Delete(string Id)
        {
            return await _mediador.Send(new EliminarRolCommand() { Id = Id });
        }
    }
}
