using System;
using System.Collections.Generic;
using FBS_Core.Identity.Domain.Models.Seguridad;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeNe.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<MenuModel> GetMenu()
        {
            return _service.List();
        }

        [HttpGet("Usuario/{idUsuario}")]
        public IEnumerable<MenuModel> GetMenuUsuario(string idUsuario)
        {
            return _service.GetMenuUsuario(idUsuario);
        }

        [HttpGet("{id}")]
        public MenuModel GetMenu(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public MenuModel Create([FromBody] MenuModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public MenuModel Update([FromBody] MenuModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public MenuModel Delete(int Id)
        {
            var result = _service.Delete(Id);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
