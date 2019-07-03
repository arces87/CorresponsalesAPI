using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeNe.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IServicioMenu _servicio;

        public MenuController(IServicioMenu service)
        {
            _servicio = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ModeloMenu>> GetMenu()
        {
            return await _servicio.List();
        }

        [HttpGet("Usuario/{idUsuario}")]
        public async Task<IEnumerable<ModeloMenu>> GetMenuUsuario(string idUsuario)
        {
            return await _servicio.GetMenuUsuario(idUsuario);
        }

        [HttpGet("{id}")]
        public async Task<ModeloMenu> GetMenu(int Id)
        {
            return await _servicio.Get(Id);
        }

        [HttpPost]
        public async Task<ModeloMenu> Create([FromBody] ModeloMenu model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<ModeloMenu> Update([FromBody] ModeloMenu model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<ModeloMenu> Delete(int Id)
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
