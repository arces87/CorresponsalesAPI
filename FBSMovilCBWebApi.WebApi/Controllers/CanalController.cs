using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Canal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CanalController : Controller
    {
        private readonly IMediator _mediador;

        public CanalController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [AllowAnonymous]
        [HttpPost("obtenerRequisitos", Name = "Canal_ObtenerRequisitosCanal")]
        public async Task<ActionResult<RequisitosCanalMS>> ObtenerJsonNegocio([FromBody] ObtenerRequisitoCanalME modelo)
        {
            var requisitos = await _mediador.Send(modelo);
            return requisitos;
        }

    }
}
