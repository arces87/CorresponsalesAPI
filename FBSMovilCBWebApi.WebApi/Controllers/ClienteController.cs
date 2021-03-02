using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

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
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> CrearCliente([FromBody] CrearClienteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarCliente", Name = "Cliente_BuscarCliente")]
        [Produces(typeof(InformacionPersonaMS))]
        public async Task<ActionResult<InformacionPersonaMS>> BuscarCliente([FromBody] BuscarClienteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("buscarTiposIdentificaciones", Name = "Cliente_TiposIdentificaciones")]
        [Produces(typeof(TiposIdentificacionMSL))]
        public async Task<ActionResult<TiposIdentificacionMSL>> BuscarTiposIdentificaciones([FromBody] ListarTiposIdentificacionME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
