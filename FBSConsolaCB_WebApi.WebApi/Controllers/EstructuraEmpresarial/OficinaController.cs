using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class OficinaController : Controller
    {
        private readonly IOficinaService _service;

        public OficinaController(IOficinaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ModeloOficina>> Get()
        {
            return await _service.List();
        }

        [HttpPost("lista")]
        public async Task<ModeloFuenteDatos<ModeloOficina>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _service.List(filtro);
        }

        [HttpGet("{id}")]
        public async Task<ModeloOficina> Get(int Id)
        {
            return await _service.Get(Id);
        }

        [HttpPost]
        public async Task<ModeloOficina> Create([FromBody] ModeloOficina model)
        {
            var result = await _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ModeloOficina> Update([FromBody] ModeloOficina model)
        {
            var result = await _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ModeloOficina> Delete(int Id)
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
