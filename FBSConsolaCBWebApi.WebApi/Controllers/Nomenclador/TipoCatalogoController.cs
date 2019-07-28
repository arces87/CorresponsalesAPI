using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TipoCatalogoController : Controller
    {
        private readonly IMediator _mediador;

        public TipoCatalogoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "TipoCatalogo_ListarTiposCatalogos")]
        public async Task<ActionResult<ModeloObtenerListaTipoCatalogo>> Get([FromBody] ObtenerListaTipoCatalogoQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet(Name = "TipoCatalogo_ObtenerTipoCatalogo")]
        public async Task<ActionResult<ObtenerModeloTipoCatalogo>> Get(ObtenerTipoCatalogoQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "TipoCatalogo_ActualizarTipoCatalogo")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarTipoCatalogoCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }
    }
}
