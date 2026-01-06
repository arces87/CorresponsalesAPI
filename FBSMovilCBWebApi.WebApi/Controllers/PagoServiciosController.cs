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
    [ApiController]
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

        [HttpPost("devuelveSimulacionPago", Name = "PagoServicios_DevuelveSimulacionPago")]
        public async Task<ActionResult<DevuelveSimulacionPagoResponse>> DevuelveSimulacionPago([FromBody] DevuelveSimulacionPagoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesaPagoServicio", Name = "PagoServicios_ProcesaPagoServicio")]
        public async Task<ActionResult<ProcesaPagoServicioResponse>> ProcesaPagoServicio([FromBody] ProcesaPagoServicioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesaReimprimirTransaccion", Name = "PagoServicios_ProcesaReimprimirTransaccion")]
        public async Task<ActionResult<ProcesaReimpresionPagoServicioResponse>> ProcesaReimprimirTransaccion([FromBody] ProcesaReimprimirTransaccionME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("procesaExtornarServicio", Name = "PagoServicios_ProcesaExtornarServicio")]
        public async Task<ActionResult<ProcesaExtornarServicioResponse>> ProcesaExtornarServicio([FromBody] ProcesaExtornarServicioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("devuelveCategoriasServicios", Name = "PagoServicios_DevuelveCategoriasServicios")]
        public async Task<ActionResult<DevuelveCategoriaResponse>> DevuelveCategoriasServicios([FromBody] DevuelveCategoriasServiciosME modelo)
        {
            if (modelo == null)
            {
                return BadRequest("El modelo de solicitud no puede ser null.");
            }
            return await _mediador.Send(modelo);
        }

        [HttpPost("devuelveDetalleDelServicio", Name = "PagoServicios_DevuelveDetalleDelServicio")]
        public async Task<ActionResult<DevuelveServicioDetalleResponse>> DevuelveDetalleDelServicio([FromBody] DevuelveDetalleDelServicioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("devuelveServiciosPorCategoria", Name = "PagoServicios_DevuelveServiciosPorCategoria")]
        public async Task<ActionResult<DevuelveServiciosResponse>> DevuelveServiciosPorCategoria([FromBody] DevuelveServiciosPorCategoriaME modelo)
        {
            return await _mediador.Send(modelo);
        }

    }
}
