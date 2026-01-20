using System.Threading.Tasks;
using Corresponsales.Query.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Generales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class GeneralesController : Controller
    {
        private readonly IMediator _mediador;

        public GeneralesController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("devuelveNombreEmpresa", Name = "Generales_DevuelveNombreEmpresa")]
        [Produces(typeof(DevuelveNombreEmpresaResponse))]
        public async Task<ActionResult<DevuelveNombreEmpresaResponse>> DevuelveNombreEmpresa([FromBody] DevuelveNombreEmpresaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}

