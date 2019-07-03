using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Nomenclador;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CatalogoController : Controller
    {
        private readonly ICatalogoService _service;

        public CatalogoController(ICatalogoService service)
        {
            _service = service;
        }

        [HttpGet("{tipo}")]
        public async Task<IEnumerable<ModeloCatalogo>> List(int Tipo)
        {
            return await _service.List(Tipo);
        }

        [HttpPost("lista")]
        public async Task<ModeloFuenteDatos<ModeloCatalogo>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _service.List(filtro);
        }

        [HttpGet("get/{id}")]
        public async Task<ModeloCatalogo> Get(int Id)
        {
            return await _service.Get(Id);
        }

        [HttpPost]
        public async Task<ModeloCatalogo> Create([FromBody] ModeloCatalogo model)
        {
            var result = await _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ModeloCatalogo> Update([FromBody] ModeloCatalogo model)
        {
            var result = await _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ModeloCatalogo> Delete(int Id)
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
