using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class PermisoController : Controller
    {
        private readonly IServicioPermiso _servicio;

        public PermisoController(IServicioPermiso servicio)
        {
            _servicio = servicio;
        }

        [HttpGet(Name = "Permiso_ListarPermisos")]
        public async Task<ActionResult<IEnumerable<ModeloPermiso>>> Get()
        {
            var resultado = await _servicio.List();
            return resultado.ToList();
        }
    }
}
