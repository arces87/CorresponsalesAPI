using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Roles.Commands;
using FBS.Identidad.Dominio.Servicios.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediador;

        public RoleController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Rol_ListarRoles")]
        public async Task<ActionResult<ListaRolMS>> Roles([FromBody] ListaRolME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Rol_ObtenerRol")]
        public async Task<ActionResult<ModeloObtenerRol>> GetRole([FromBody] ObtenerRolME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Rol_CrearRol")]
        public async Task<ActionResult<string>> Create([FromBody] CrearRolME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "Rol_ActualizarRol")]
        public async Task<ActionResult<string>> Update([FromBody] ModificarRolME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete(Name = "Rol_EliminarRol")]
        public async Task<ActionResult<bool>> Delete([FromBody] EliminarRolME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("verificarRol", Name = "Rol_VerificarRol")]
        public async Task<ActionResult<bool>> VerificarRol ([FromBody] ComprobarRolME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
