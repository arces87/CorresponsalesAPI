using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Identity.Domain.Models.Seguridad;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<RoleModel> Roles()
        {
            return _service.GetRoles();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<RoleModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("{Id}")]
        public RoleModel GetRole(string Id)
        {
            return _service.GetRole(Id);
        }

        [HttpPost]
        public async Task<object> Create([FromBody] RoleModel model)
        {
            var result = await _service.CreateRole(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public async Task<object> Update([FromBody] RoleModel model)
        {
            var result = await _service.UpdateRole(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public async Task<object> Delete(string Id)
        {
            var result = await _service.DeleteRole(Id);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
