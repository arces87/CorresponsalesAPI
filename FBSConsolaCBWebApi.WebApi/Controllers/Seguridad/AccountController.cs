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

        [HttpPost("lista", Name = "Usuario_ListarUsuarios")]
        public async Task<ActionResult<ListaUsuarioMS>> Listar([FromBody] ListaUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
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

        [HttpPost("SolicitarCambioContrasenia", Name = "Usuario_SolicitarCambioContrasenia")]
        public async Task<ActionResult<string>> SolicitarCambioContrasenia([FromBody] SolicitarCambioContraseniaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("CambioContrasenia", Name = "Usuario_CambioContrasenia")]
        public async Task<ActionResult<string>> CambioContrasenia([FromBody] CambioContraseniaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Usuario_CrearUsuario")]
        public async Task<ActionResult<string>> Crear([FromBody] CrearUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("ComprobarUsuario", Name = "Usuario_ComprobarUsuario")]
        public async Task<ActionResult<bool>> ComprobarUsuario([FromBody] ComprobarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("ComprobarCorreoElectronico", Name = "Usuario_ComprobarCorreoElectronico")]
        public async Task<ActionResult<bool>> Crear([FromBody] ComprobarCorreoElectronicoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut("ModificarUsuario", Name = "Usuario_ModificarUsuario")]
        public async Task<ActionResult<string>> Update([FromBody] ModificarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

    }
}
