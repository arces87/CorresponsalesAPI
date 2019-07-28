using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly IMediator _mediador;

        public LogController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Log_ListarLogs")]
        public async Task<ActionResult<ModeloObtenerListaLog>> List([FromBody] ObtenerListaLogQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet("get/{id}", Name = "Log_ObtenerLog")]
        public async Task<ActionResult<ObtenerModeloLog>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerLogQuery() { Id = Id });
        }
    }
}
