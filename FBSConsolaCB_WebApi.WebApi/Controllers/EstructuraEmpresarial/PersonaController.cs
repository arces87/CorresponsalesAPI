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

        [HttpPost("lista/corresponsales")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloCorresponsal>>> ListaCorresponsales([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.ListaCorresponsales(filtro);
        }

        [HttpPost("lista/supervisores")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloSupervisor>>> ListaSupervisores([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.ListaSupervisores(filtro);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(int Id)
        {
            var temp = await _servicio.Get(Id);
            return temp;
        }

        [HttpPost("corresponsal")]
        public async Task<ActionResult<ModeloCorresponsal>> Create([FromBody] ModeloCorresponsal model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPost("supervisor")]
        public async Task<ActionResult<ModeloSupervisor>> Create([FromBody] ModeloSupervisor model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut("corresponsal")]
        public async Task<ActionResult<ModeloCorresponsal>> Update([FromBody] ModeloCorresponsal model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }


        [HttpPut("supervisor")]
        public async Task<ActionResult<ModeloSupervisor>> Update([FromBody] ModeloSupervisor model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
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
