using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly IMediator _mediador;

        public LogController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet("listarLogs", Name = "Log_ListarLog")]
        public async Task<ActionResult<ModeloObtenerListaLog>> List()
        {
            return await _mediador.Send(new ObtenerListaLogQuery());
        }

        [HttpGet("obtenerLog/{id}", Name = "Log_ObtenerLog")]
        public async Task<ActionResult<ObtenerModeloLog>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerLogQuery() { Id = Id });
        }

        [HttpPost("crearLog", Name = "Log_RegistrarLog")]
        public async Task<ActionResult<int>> Crear([FromBody] CrearLogCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
