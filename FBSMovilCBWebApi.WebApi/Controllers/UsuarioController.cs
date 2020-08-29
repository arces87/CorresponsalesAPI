using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IMediator _mediador;

        public UsuarioController(IMediator mediador)
        {
            _mediador = mediador;
        }


        [HttpPost("login", Name = "Usuario_AutenticarUsuario")]
        [Produces(typeof(AutenticarUsuarioMS))]
        public async Task<ActionResult<AutenticarUsuarioMS>> Login([FromBody] AutenticarUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("solicitudActivacion", Name = "Usuario_SolicitudActivacion")]
        public async Task<ActionResult> Activacion([FromBody] SolicitarActivacionME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("solicitudOtp", Name = "Usuario_SolicitarOtp")]
        public async Task<ActionResult<SolicitarOtpMS>> SolicitarOtp([FromBody] SolicitarOtpME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("verificarOtp", Name = "Usuario_VerificarOtp")]
        public async Task<ActionResult> VerificarOtp([FromBody] VerificarOtpME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("CambioContrasenia", Name = "Usuario_CambioContrasenia")]
        public async Task<ActionResult<bool>> CambioContrasenia([FromBody] CambioContraseniaME modelo)
        {
            await _mediador.Send(modelo);
            return Ok(true);
        }
    }
}
