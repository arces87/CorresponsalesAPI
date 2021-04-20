using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Menus.Commands;
using FBS.Identidad.Dominio.Servicios.Menus.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class MenuController : Controller
    {
        private readonly IMediator _mediador;

        public MenuController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet("lista", Name = "Menu_ListarMenus")]
        public async Task<ActionResult<ListaMenuMS>> GetMenu()
        {
            return await _mediador.Send(new ListaMenuME());
        }

        [HttpPost("Usuario", Name = "Menu_ObtenerMenusUsuario")]
        public async Task<ActionResult<ListaMenuUsuarioMS>> GetMenuUsuario([FromBody] ListaMenuUsuarioME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Menu_ObtenerMenu")]
        public async Task<ActionResult<ObtenerMenuMS>> GetMenu([FromBody] ObtenerMenuME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Menu_CrearMenu")]
        public async Task<ActionResult<string>> Create([FromBody] CrearMenuME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "Menu_ActualizarMenu")]
        public async Task<ActionResult<string>> Update([FromBody] ModificarMenuME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete(Name = "Menu_EliminarMenu")]
        public async Task<ActionResult<bool>> Delete([FromBody] EliminarMenuME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
