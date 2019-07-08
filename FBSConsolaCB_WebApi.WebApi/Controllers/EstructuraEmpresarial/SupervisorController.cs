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
    public class SupervisorController : Controller
    {
        private readonly IServicioPersona _servicio;

        public SupervisorController(IServicioPersona servicio)
        {
            _servicio = servicio;
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ModeloFuenteDatos<ModeloSupervisor>>> ListaSupervisores([FromBody] ModeloPaginacion filtro)
        {
            return await _servicio.ListaSupervisores(filtro);
        }

        [HttpPost]
        public async Task<ActionResult<ModeloSupervisor>> Create([FromBody] ModeloSupervisor model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ActionResult<ModeloSupervisor>> Update([FromBody] ModeloSupervisor model)
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
