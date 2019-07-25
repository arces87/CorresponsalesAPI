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
        [Produces(typeof(ModeloUsuarioAutenticado))]
        //[SwaggerResponse(operationId: "getA")]
        public async Task<ActionResult<ModeloUsuarioAutenticado>> Login([FromBody] AutenticarUsuarioCommand modelo)
        {
            var result = await _mediador.Send(modelo);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }
    }
}
