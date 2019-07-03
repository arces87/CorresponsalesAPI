using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LimiteTransaccionalController : Controller
    {
        private readonly IServicioLimiteTransaccional _servicio;

        public LimiteTransaccionalController(IServicioLimiteTransaccional servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<IEnumerable<ModeloLimiteTransaccional>> List()
        {
            return await _servicio.List();
        }

        [HttpPost("lista")]
        public async Task<ModeloFuenteDatos<ModeloLimiteTransaccional>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }

        [HttpGet("get/{id}")]
        public async Task<ModeloLimiteTransaccional> Get(int Id)
        {
            return await _servicio.Get(Id);
        }

        [HttpPost]
        public async Task<ModeloLimiteTransaccional> Create([FromBody] ModeloLimiteTransaccional model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ModeloLimiteTransaccional> Update([FromBody] ModeloLimiteTransaccional model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ModeloLimiteTransaccional> Delete(int Id)
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
