using System.Threading.Tasks;
using Corresponsales.Query.Model;
using FBSConsolaCBWebApi.Dominio.Servicios.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClienteController : Controller
    {
        private readonly IMediator _mediador;

        public ClienteController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("buscarTiposIdentificaciones", Name = "Cliente_BuscarTiposIdentificaciones")]
        [Produces(typeof(DevuelveTiposIdentificacionResponse))]
        public async Task<ActionResult<DevuelveTiposIdentificacionResponse>> BuscarTiposIdentificaciones([FromBody] ListarTiposIdentificacionME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
