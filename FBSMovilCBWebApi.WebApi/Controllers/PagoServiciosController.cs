using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;
using ObtenerFormatosME = FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries.ObtenerFormatosME;

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
        public async Task<ActionResult<AfectacionMS>> PagarServicio([FromBody] ProcesarPagoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet("obtenerServicios", Name = "PagoServicios_ObtenerServicios")]
        public async Task<ActionResult<ServiciosMSL>> ObtenerServicios([FromBody] ObtenerServiciosME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtenerFormatos", Name = "PagoServicios_ObtenerFormatos")]
        public async Task<ActionResult<FormatoMS>> ObtenerProductos([FromBody] ObtenerFormatosME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("consultaServicio", Name = "PagoServicios_ConsultaServicio")]
        public async Task<ActionResult<ConsultaMS>> ConsultaServicio([FromBody] ConsultaServiciosME modelo)
        {
            return await _mediador.Send(modelo);
        }

    }
}
