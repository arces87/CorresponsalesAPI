using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

namespace GeNe.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PermisoController : Controller
    {
        private readonly IServicioPermiso _servicio;

        public PermisoController(IServicioPermiso service)
        {
            _servicio = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ModeloPermiso>> Get()
        {
            return await _servicio.List();
        }

        [HttpPost("lista")]
        public async Task<ModeloFuenteDatos<ModeloPermiso>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }
    }
}
