using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DistribuidosController : Controller
    {
        private readonly IMediator _mediador;

        public DistribuidosController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("obtenerDistribuidos", Name = "Distribuidos_ObtenerDistribuidos")]
        [Produces(typeof(ObtenerDistribuidosMS))]
        public async Task<ActionResult<ObtenerDistribuidosMS>> BuscarDistribuidos([FromBody] ObtenerDistribuidosME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
