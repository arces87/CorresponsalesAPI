using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TipoCatalogoController : Controller
    {
        private readonly IMediator _mediador;

        public TipoCatalogoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "TipoCatalogo_ListarTiposCatalogos")]
        public async Task<ActionResult<ListarTipoCatalogoMS>> Get([FromBody] ListarTipoCatalogoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "TipoCatalogo_ObtenerTipoCatalogo")]
        public async Task<ActionResult<ObtenerTipoCatalogoMS>> Get([FromBody] ObtenerTipoCatalogoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "TipoCatalogo_ActualizarTipoCatalogo")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarTipoCatalogoME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
