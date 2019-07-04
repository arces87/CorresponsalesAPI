using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly IServicioLog _servicio;

        public LogController(IServicioLog servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloLog>>> List()
        {
            var resultado = await _servicio.List();
            return resultado.ToList();
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloLog>>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ModeloLog>> Get(int Id)
        {
            return await _servicio.Get(Id);
        }

        [HttpPost]
        public async Task<ActionResult<ModeloLog>> Create([FromBody] ModeloLog model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
