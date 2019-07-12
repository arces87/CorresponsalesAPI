using System;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PersonaController : Controller
    {
        private readonly IServicioPersona _servicio;

        public PersonaController(IServicioPersona servicio)
        {
            _servicio = servicio;
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloPersona>>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(int Id)
        {
            var temp = await _servicio.Get(Id);
            return temp;
        }

        [HttpGet("DevuelveDatosPersonaIdentificacion")]
        public Task<ModeloPersona> DevuelveDatosPersonaIdentificacion(string identificacion)
        {
            var rusultado = _servicio.DevuelveDatosPersonaIdentificacion(identificacion);
            return rusultado;
        }

        [HttpGet("CambiarEstadoCorresponsal")]
        public Task<ModeloCorresponsal> CambiarEstadoCorresponsal(int id)
        {
            var rusultado = _servicio.CambiarEstadoCorresponsal(id);
            return rusultado;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ModeloPersona>> Delete(int Id)
        {
            var result = await _servicio.Delete(Id);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
