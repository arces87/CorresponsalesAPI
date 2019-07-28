using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Menus.Commands;
using FBS.Identidad.Dominio.Servicios.Menus.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IMediator _mediador;

        public MenuController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet(Name = "Menu_ListarMenus")]
        public async Task<ActionResult<ModeloObtenerListaMenu>> GetMenu()
        {
            return await _mediador.Send(new ObtenerListaMenuQuery());
        }

        [HttpGet("Usuario/{idUsuario}", Name = "Menu_ObtenerMenusUsuario")]
        public async Task<ActionResult<ModeloObtenerListaMenuUsuario>> GetMenuUsuario(string idUsuario)
        {
            return await _mediador.Send(new ObtenerListaMenuUsuarioQuery() { IdUsuario = idUsuario });
        }

        [HttpGet("{id}", Name = "Menu_ObtenerMenu")]
        public async Task<ActionResult<ObtenerModeloMenu>> GetMenu(int Id)
        {
            return await _mediador.Send(new ObtenerMenuQuery() { Id = Id });
        }

        [HttpPost(Name = "Menu_CrearMenu")]
        public async Task<ActionResult<int>> Create([FromBody] CrearMenuCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut(Name = "Menu_ActualizarMenu")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarMenuCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}", Name = "Menu_EliminarMenu")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarMenuCommand() { Id = Id });
        }
    }
}
