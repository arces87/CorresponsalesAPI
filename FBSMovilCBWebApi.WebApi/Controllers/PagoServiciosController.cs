using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Facilito.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;
using ObtenerProductosME = FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries.ObtenerProductosME;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    [Authorize]
    public class PagoServiciosController : Controller
    {
        private readonly IMediator _mediador;

        public PagoServiciosController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("pagarServicio", Name = "PagoServicios_PagarServicio")]
        public async Task<ActionResult<PagoFacilitoMSL>> PagarServicio([FromBody] ProcesarPagoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet("obtenerServicios", Name = "PagoServicios_ObtenerServicios")]
        public async Task<ActionResult<ObtenerServiciosMS>> ObtenerServicios()
        {
            return await _mediador.Send(new ObtenerServiciosME());
        }

        [HttpPost("obtenerProductos", Name = "PagoServicios_ObtenerProductos")]
        public async Task<ActionResult<ObtenerProductosMS>> ObtenerProductos([FromBody] ObtenerProductosME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("consultaServicio", Name = "PagoServicios_ConsultaServicio")]
        public async Task<ActionResult<ConsultaValorAPagarMS>> ConsultaServicio([FromBody] ConsultaServiciosME modelo)
        {
            return await _mediador.Send(modelo);
        }

    }
}
