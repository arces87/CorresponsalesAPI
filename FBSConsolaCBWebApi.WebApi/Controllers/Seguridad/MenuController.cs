using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IServicioMenu _servicio;

        public MenuController(IServicioMenu servicio)
        {
            _servicio = servicio;
        }

        [HttpGet (Name = "Menu_ListarMenus")]
        public async Task<ActionResult<IEnumerable<ModeloMenu>>> GetMenu()
        {
            var resultado = await _servicio.List();
            return resultado.ToList();
        }

        [HttpGet("Usuario/{idUsuario}", Name = "Menu_ObtenerMenusUsuario")]
        public async Task<ActionResult<IEnumerable<ModeloMenu>>> GetMenuUsuario(string idUsuario)
        {
            var resultado = await _servicio.GetMenuUsuario(idUsuario);
            return resultado.ToList();
        }

        [HttpGet("{id}", Name = "Menu_ObtenerMenu")]
        public async Task<ActionResult<ModeloMenu>> GetMenu(int Id)
        {
            return await _servicio.Get(Id);
        }

        [HttpPost(Name = "Menu_CrearMenu")]
        public async Task<ActionResult<ModeloMenu>> Create([FromBody] ModeloMenu model)
        {
            var result = await _servicio.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut(Name = "Menu_ActualizarMenu")]
        public async Task<ActionResult<ModeloMenu>> Update([FromBody] ModeloMenu model)
        {
            var result = await _servicio.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}", Name = "Menu_EliminarMenu")]
        public async Task<ActionResult<ModeloMenu>> Delete(int Id)
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
