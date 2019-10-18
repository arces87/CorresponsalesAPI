using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AlertaController : Controller
    {
        private readonly IMediator _mediador;

        public AlertaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("listarAlertas", Name = "Alerta_ListarAlertas")]
        public async Task<ActionResult<ListarAlertaMS>> List([FromBody] ListarAlertaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("crearAlerta", Name = "Alerta_CrearAlerta")]
        public async Task<ActionResult<string>> Create([FromBody] CrearAlertaME model)
        {
            return await _mediador.Send(model);
        }
    }
}
