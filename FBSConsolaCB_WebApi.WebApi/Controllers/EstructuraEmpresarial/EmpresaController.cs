using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EmpresaController : Controller
    {
        private readonly IServicioEmpresa _servicio;

        public EmpresaController(IServicioEmpresa servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloEmpresa>>> Get()
        {
            var resultado = await _servicio.List();
            return resultado.ToList();
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloEmpresa>>> Get([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.List(filtro);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModeloEmpresa>> Get(int Id)
        {
            return await _servicio.Get(Id);
        }

        [HttpPost]
        public async Task<ActionResult<ModeloEmpresa>> Create([FromBody] ModeloEmpresa model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ActionResult<ModeloEmpresa>> Update([FromBody] ModeloEmpresa model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ModeloEmpresa>> Delete(int Id)
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
