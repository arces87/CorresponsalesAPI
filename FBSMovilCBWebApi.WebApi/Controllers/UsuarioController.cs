using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
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
    }
}
