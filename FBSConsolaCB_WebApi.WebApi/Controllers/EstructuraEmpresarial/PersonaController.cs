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
        private readonly IPersonaService _service;

        public PersonaController(IPersonaService service)
        {
            _service = service;
        }

        [HttpPost("lista")]
        public async Task<ModeloFuenteDatos<ModeloPersona>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _service.List(filtro);
        }


        [HttpGet("{id}")]
        public async Task<object> Get(int Id)
        {
            var temp = await _service.Get(Id);
            return temp;
        }

        [HttpPost("corresponsal")]
        public async Task<ModeloCorresponsal> Create([FromBody] ModeloCorresponsal model)
        {
            var result = await _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPost("supervisor")]
        public async Task<ModeloSupervisor> Create([FromBody] ModeloSupervisor model)
        {
            var result = await _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut("corresponsal")]
        public async Task<ModeloCorresponsal> Update([FromBody] ModeloCorresponsal model)
        {
            var result = await _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }


        [HttpPut("supervisor")]
        public async Task<ModeloSupervisor> Update([FromBody] ModeloSupervisor model)
        {
            var result = await _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ModeloPersona> Delete(int Id)
        {
            var result = await _service.Delete(Id);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
