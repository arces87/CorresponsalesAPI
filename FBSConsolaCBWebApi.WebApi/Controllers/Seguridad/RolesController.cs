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

        [HttpPost("lista", Name = "Rol_ListarRoles")]
        public async Task<ActionResult<ModeloObtenerListaRol>> Roles([FromBody] ObtenerListaRolQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Rol_ObtenerRol")]
        public async Task<ActionResult<ObtenerModeloRol>> GetRole([FromBody] ObtenerRolQuery modelo)
        {
            return await _mediador.Send(modelo);
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

        [HttpDelete(Name = "Rol_EliminarRol")]
        public async Task<ActionResult<bool>> Delete([FromBody] EliminarRolCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
