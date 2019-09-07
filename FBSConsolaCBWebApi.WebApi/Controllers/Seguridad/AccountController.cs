using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Identidad.Dominio.Servicios.Usuarios.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediador;

        public AccountController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet("lista", Name = "Usuario_ListarUsuarios")]
        public async Task<ActionResult<ListaUsuarioMS>> Listar()
        {
            return await _mediador.Send(new ListaUsuarioME());
        }

        [HttpPost("obtener", Name = "Usuario_ObtenerUsuario")]
        public async Task<ActionResult<ModeloObtenerUsuario>> GetUser([FromBody] ObtenerUsuarioME modelo)
        {
            var usuario = await _mediador.Send(modelo);
            return usuario;
        }

        [HttpPost("Login", Name = "Usuario_AutenticarUsuario")]
        public async Task<ActionResult<AutenticarUsuarioMS>> Login([FromBody] AutenticarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Usuario_CrearUsuario")]
        public async Task<ActionResult<string>> Crear([FromBody] CrearUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] ModificarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

    }
}
