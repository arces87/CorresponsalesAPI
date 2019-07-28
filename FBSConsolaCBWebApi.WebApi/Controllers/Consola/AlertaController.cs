using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlertaController : Controller
    {
        private readonly IMediator _mediador;

        public AlertaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Alerta_ListarAlertas")]
        public async Task<ActionResult<ModeloObtenerListaAlerta>> List([FromBody] ObtenerListaAlertaQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet(Name = "Alerta_ObtenerAlerta")]
        public async Task<ActionResult<ObtenerModeloAlerta>> Get(ObtenerAlertaQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Alerta_CrearAlerta")]
        public async Task<ActionResult<int>> Create([FromBody] CrearAlertaCommand model)
        {
            return await _mediador.Send(model);
        }

        [HttpDelete(Name = "Alerta_EliminarAlerta")]
        public async Task<ActionResult<int>> Delete(EliminarAlertaCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
