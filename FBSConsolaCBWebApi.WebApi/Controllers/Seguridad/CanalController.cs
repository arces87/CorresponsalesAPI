using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class CanalController : Controller
    {
        private readonly IMediator _mediador;

        public CanalController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("obtenerJsonNegocio", Name = "Usuario_ObtenerJsonNegocio")]
        public async Task<ActionResult<JsonNegocioMS>> ObtenerJsonNegocio([FromBody] ObtenerJsonNegocioME modelo)
        {
            var usuario = await _mediador.Send(modelo);
            return usuario;
        }
    }
}
