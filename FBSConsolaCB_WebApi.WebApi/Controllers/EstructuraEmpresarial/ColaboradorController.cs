using System;
using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CorresponsalController : Controller
    {
        private readonly ICorresponsalService _service;

        public CorresponsalController(ICorresponsalService service)
        {
            _service = service;
        }

        [HttpPost("lista")]
        public FuenteDatosModel<CorresponsalModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }


        [HttpGet("{id}")]
        public object Get(int Id)
        {
            var temp = _service.Get(Id);
            return temp;
        }

        [HttpPost]
        public CorresponsalModel Create([FromBody] CorresponsalModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public CorresponsalModel Update([FromBody] CorresponsalModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public CorresponsalModel Delete(int Id)
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
