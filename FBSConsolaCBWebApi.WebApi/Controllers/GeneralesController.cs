using System.Threading.Tasks;
using Corresponsales.Query.Model;
using FBSConsolaCBWebApi.Dominio.Servicios.Generales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FBSConsolaCBWebApi.WebApi.Controllers
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
        public async Task<ActionResult<DevuelveNombreEmpresaResponse>> DevuelveNombreEmpresa([FromBody] DevuelveNombreEmpresaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}

