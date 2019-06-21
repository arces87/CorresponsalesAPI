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
    public class OficinaController : Controller
    {
        private readonly IOficinaService _service;

        public OficinaController(IOficinaService service)
        {
            _service = service;
        }

        [HttpGet]
       // [Authorize(Policy = "Listar Oficina")]
        public IEnumerable<OficinaModel> Get()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<OficinaModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("{id}")]
        public OficinaModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public OficinaModel Create([FromBody] OficinaModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public OficinaModel Update([FromBody] OficinaModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public OficinaModel Delete(int Id)
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
