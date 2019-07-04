using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IServicioRol _servicio;

        public RoleController(IServicioRol servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloRol>>> Roles()
        {
            var resultado = await _servicio.GetRoles();
            return resultado.ToList();
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloRol>>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ModeloRol>> GetRole(string Id)
        {
            return await _servicio.GetRole(Id);
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] ModeloRol model)
        {
            var result = await _servicio.CreateRole(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ActionResult<object>> Update([FromBody] ModeloRol model)
        {
            var result = await _servicio.UpdateRole(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(string Id)
        {
            var result = await _servicio.DeleteRole(Id);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
