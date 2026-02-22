using System.Threading.Tasks;
using Corresponsales.Command.Model;
using Corresponsales.Query.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class ClienteController : Controller
    {
        private readonly IMediator _mediador;

        public ClienteController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("crearCliente", Name = "Cliente_CrearCliente")]
        [Produces(typeof(CreaClienteResponse))]
        public async Task<ActionResult<CreaClienteResponse>> CrearCliente([FromBody] CrearClienteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarCliente", Name = "Cliente_BuscarCliente")]
        [Produces(typeof(DevuelveDatosPersonaIdentificacionResponse))]
        public async Task<ActionResult<DevuelveDatosPersonaIdentificacionResponse>> BuscarCliente([FromBody] BuscarClienteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarTiposIdentificaciones", Name = "Cliente_TiposIdentificaciones")]
        [Produces(typeof(DevuelveTiposIdentificacionResponse))]
        public async Task<ActionResult<DevuelveTiposIdentificacionResponse>> BuscarTiposIdentificaciones([FromBody] ListarTiposIdentificacionME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
