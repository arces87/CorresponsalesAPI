using System;
using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands;
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

        [HttpPost("login", Name = "Usuario_Login")]
        [Produces(typeof(ProcesarLoginMS))]
        public async Task<ActionResult<ProcesarLoginMS>> Login([FromBody] DatosLoginME modelo)
        {
            try
            {
                return await _mediador.Send(modelo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("generarOtp", Name = "Usuario_GenerarOtp")]
        [Produces(typeof(ProcesarLoginMS))]
        public async Task<ActionResult<ProcesarOtpMS>> ComprobarOtp([FromBody] DatosOtpME modelo)
        {
            try
            {
                return await _mediador.Send(modelo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("comprobarOtp", Name = "Usuario_ComprobarOtp")]
        [Produces(typeof(ProcesarLoginMS))]
        public async Task<ActionResult<ProcesarValidarOtpMS>> GenerarOtp([FromBody] DatosValidarOTPME modelo)
        {
            try
            {
                return await _mediador.Send(modelo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
