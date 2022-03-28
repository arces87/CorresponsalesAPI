using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Identidad.Dominio.Servicios.Usuarios.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediador;

        public AccountController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [Authorize]
        [HttpPost("lista", Name = "Usuario_ListarUsuarios")]
        public async Task<ActionResult<ListaUsuarioMS>> Listar([FromBody] ListaUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [Authorize]
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
        public async Task<ActionResult<bool>> SolicitarCambioContrasenia([FromBody] SolicitarCambioContraseniaME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("CambioContrasenia", Name = "Usuario_CambioContrasenia")]
        public async Task<ActionResult<string>> CambioContrasenia([FromBody] CambioContraseniaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpPost(Name = "Usuario_CrearUsuario")]
        public async Task<ActionResult<string>> Crear([FromBody] CrearUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpPost("ComprobarUsuario", Name = "Usuario_ComprobarUsuario")]
        public async Task<ActionResult<bool>> ComprobarUsuario([FromBody] ComprobarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpPost("ComprobarCorreoElectronico", Name = "Usuario_ComprobarCorreoElectronico")]
        public async Task<ActionResult<bool>> Crear([FromBody] ComprobarCorreoElectronicoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpPut("ModificarUsuario", Name = "Usuario_ModificarUsuario")]
        public async Task<ActionResult<string>> Update([FromBody] ModificarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpPost("verificarCodigoUsuario", Name = "Usuario_VerificarCodigoUsuario")]
        public async Task<ActionResult<bool>> VerificarCodigoUsuario([FromBody] VerificarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpPost("verificarCorreoElectronico", Name = "Usuario_VerificarCorreoElectronico")]
        public async Task<ActionResult<bool>> VerificarCorreoElectronico([FromBody] VerificarCorreoElectronicoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [Authorize]
        [HttpDelete("eliminar", Name = "Usuario_BloquearUsuario")]
        public async Task<ActionResult<bool>> Delete([FromBody] EliminarUsuarioME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
