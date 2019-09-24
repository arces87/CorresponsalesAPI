using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly IMediator _mediador;

        public ClienteController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("crearCliente", Name = "Cliente_CrearCliente")]
        [Produces(typeof(CrearClienteMS))]
        public async Task<ActionResult<CrearClienteMS>> CrearCliente([FromBody] CrearClienteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarCliente", Name = "Cliente_BuscarCliente")]
        [Produces(typeof(BuscarClienteMS))]
        public async Task<ActionResult<BuscarClienteMS>> BuscarCliente([FromBody] BuscarClienteME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
