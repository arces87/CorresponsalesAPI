using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IServicioUsuarioLocal _servicio;

        public AccountController(IServicioUsuarioLocal servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public IEnumerable<ModeloUsuario> Users()
        {
            return _servicio.Users();
        }
        [HttpPost("lista")]
        public ModeloFuenteDatos<ModeloUsuario> Get([FromBody] ModeloPaginacion filtro)
        {
            return _servicio.List(filtro);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(string Id)
        {
            return await _servicio.GetUser(Id);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ModeloPersona>> Login([FromBody] ModeloUsuario model)
        {
            var result = await _servicio.Autenticar(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost]
        public async Task<ActionResult<object>> Register([FromBody] ModeloUsuario model)
        {
            var result = await _servicio.CreateUser(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPut]
        public async Task<ActionResult<object>> Update([FromBody] ModeloUsuario model)
        {
            var result = await _servicio.UpdateUser(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_USER_DATA");
        }

    }
}
