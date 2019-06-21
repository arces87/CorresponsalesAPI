using System;
using System.Collections.Generic;
using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AreaTrabajoController : Controller
    {
        private readonly IAreaTrabajoService _service;

        public AreaTrabajoController(IAreaTrabajoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<AreaTrabajoModel> Get()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<AreaTrabajoModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("{id}")]
        public AreaTrabajoModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public AreaTrabajoModel Create([FromBody] AreaTrabajoModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public AreaTrabajoModel Update([FromBody] AreaTrabajoModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public AreaTrabajoModel Delete(int Id)
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
