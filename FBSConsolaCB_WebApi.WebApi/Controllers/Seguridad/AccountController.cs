using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeNe.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IServicioUsuarioLocal _service;

        public AccountController(IServicioUsuarioLocal service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ModeloUsuario> Users()
        {
            return _service.Users();
        }
        [HttpPost("lista")]
        public ModeloFuenteDatos<ModeloUsuario> Get([FromBody] ModeloPaginacion filtro)
        {
            return _service.List(filtro);
        }
        [HttpGet("{id}")]
        public async Task<object> GetUser(string Id)
        {
            return await _service.GetUser(Id);
        }

        [HttpPost("Login")]
        public async Task<ModeloPersona> Login([FromBody] ModeloUsuario model)
        {
            var result = await _service.Autenticar(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost]
        public async Task<object> Register([FromBody] ModeloUsuario model)
        {
            var result = await _service.CreateUser(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPut]
        public async Task<object> Update([FromBody] ModeloUsuario model)
        {
            var result = await _service.UpdateUser(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_USER_DATA");
        }

    }
}
