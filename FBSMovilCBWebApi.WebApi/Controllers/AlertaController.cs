using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlertaController : Controller
    {
        private readonly IMediator _mediador;

        public AlertaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet]
        public async Task<ActionResult<ModeloObtenerListaAlerta>> List()
        {
            return await _mediador.Send(new ObtenerListaAlertaQuery());
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ObtenerModeloAlerta>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerAlertaQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearAlertaCommand model)
        {
            return await _mediador.Send(model);
        }
    }
}
