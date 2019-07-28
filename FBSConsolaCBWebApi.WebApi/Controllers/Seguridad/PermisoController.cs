using System.Threading.Tasks;
using FBS.Identidad.Dominio.Servicios.Permisos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class PermisoController : Controller
    {
        private readonly IMediator _mediador;

        public PermisoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet(Name = "Permiso_ListarPermisos")]
        public async Task<ActionResult<ModeloObtenerListaPermiso>> Get()
        {
            return await _mediador.Send(new ObtenerListaPermisoQuery());
        }
    }
}
