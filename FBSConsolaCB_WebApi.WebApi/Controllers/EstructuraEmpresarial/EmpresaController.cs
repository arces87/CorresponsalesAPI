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
    public class EmpresaController : Controller
    {
        private readonly IEmpresaService _service;

        public EmpresaController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<EmpresaModel> Get()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<EmpresaModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("{id}")]
        public EmpresaModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public EmpresaModel Create([FromBody] EmpresaModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public EmpresaModel Update([FromBody] EmpresaModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public EmpresaModel Delete(int Id)
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
