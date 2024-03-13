using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObtenerProductoME = FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries.ObtenerProductoME;
using Corresponsales.Command.Model;
using Corresponsales.Query.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class PagoServiciosController : Controller
    {
        private readonly IMediator _mediador;

        public PagoServiciosController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("pagarServicio", Name = "PagoServicios_PagarServicio")]
        public async Task<ActionResult<PagoFacilitoResponse>> PagarServicio([FromBody] ProcesarPagoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtenerServicios", Name = "PagoServicios_ObtenerServicios")]
        public async Task<ActionResult<ObtenerServiciosResponse>> ObtenerServicios([FromBody] ObtenerServiciosME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtenerProductos", Name = "PagoServicios_ObtenerProductos")]
        public async Task<ActionResult<ObtenerProductosResponse>> ObtenerProductos([FromBody] ObtenerProductoME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("consultaServicio", Name = "PagoServicios_ConsultaServicio")]
        public async Task<ActionResult<ConsultaValorAPagarResponse>> ConsultaServicio([FromBody] ConsultaServiciosME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("reversoFacilito", Name = "PagoServicios_ReversoFacilito")]
        public async Task<ActionResult<ReversoFacilitoResponse>> ReversoFacilito([FromBody] ReversoME modelo)
        {
            return await _mediador.Send(modelo);
        }

    }
}
