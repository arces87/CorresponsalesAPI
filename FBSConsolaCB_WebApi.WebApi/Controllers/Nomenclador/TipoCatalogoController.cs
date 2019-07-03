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
    public class TipoCatalogoController : Controller
    {
        private readonly ITipoCatalogoService _service;

        public TipoCatalogoController(ITipoCatalogoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ModeloTipoCatalogo>> Get()
        {
            return await _service.List();
        }

        [HttpPost("lista")]
        public async Task<ModeloFuenteDatos<ModeloTipoCatalogo>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _service.List(filtro);
        }

        [HttpGet("{id}")]
        public async Task<ModeloTipoCatalogo> Get(int Id)
        {
            return await _service.Get(Id);
        }

        [HttpPut]
        public async Task<object> Update([FromBody] ModeloTipoCatalogo model)
        {
            var result = await _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
