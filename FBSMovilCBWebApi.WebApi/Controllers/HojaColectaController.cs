using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class HojaColectaController : Controller
    {
        private readonly IMediator _mediador;

        public HojaColectaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("cerrarDia", Name = "HoaColecta_CerrarDia")]
        public async Task<ActionResult> CerrarDia([FromBody] CerrarDiaME modelo)
        {
            await _mediador.Send(modelo);
            return Ok();
        }
    }
}
