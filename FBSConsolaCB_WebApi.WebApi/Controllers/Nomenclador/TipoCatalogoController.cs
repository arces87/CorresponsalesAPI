using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IServicioTipoCatalogo _servicio;

        public TipoCatalogoController(IServicioTipoCatalogo servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloTipoCatalogo>>> Get()
        {
            var resultado = await _servicio.List();
            return resultado.ToList();
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloTipoCatalogo>>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModeloTipoCatalogo>> Get(int Id)
        {
            return await _servicio.Get(Id);
        }

        [HttpPut]
        public async Task<ActionResult<ModeloTipoCatalogo>> Update([FromBody] ModeloTipoCatalogo model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
