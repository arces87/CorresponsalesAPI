using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Identity.Domain.Models.Seguridad;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeNe.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _service;

        public AccountController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<UserModel> Users()
        {
            return _service.Users();
        }
        [HttpPost("lista")]
        public FuenteDatosModel<UserModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }
        [HttpGet("{id}")]
        public async Task<object> GetUser(string Id)
        {
            return await _service.GetUser(Id);
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] UserModel model)
        {
            var result = await _service.Login(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost]
        public async Task<object> Register([FromBody] UserModel model)
        {
            var result = await _service.CreateUser(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPut]
        public async Task<object> Update([FromBody] UserModel model)
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
