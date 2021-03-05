using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AlertaController : Controller
    {
        private readonly IMediator _mediador;

        public AlertaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Alerta_ListarAlertas")]
        public async Task<ActionResult<ListarAlertaMS>> List([FromBody] ListarAlertaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Alerta_ObtenerAlerta")]
        public async Task<ActionResult<ObtenerAlertaMS>> Get([FromBody] ObtenerAlertaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut("actualizar", Name = "Alerta_ActualizarAlerta")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarAlertaME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
