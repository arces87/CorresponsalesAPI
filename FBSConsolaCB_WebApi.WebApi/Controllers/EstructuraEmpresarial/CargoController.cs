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
    public class CargoController : Controller
    {
        private readonly ICargoService _service;

        public CargoController(ICargoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<CargoModel> Get()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<CargoModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("{id}")]
        public CargoModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public CargoModel Create([FromBody] CargoModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public CargoModel Update([FromBody] CargoModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public CargoModel Delete(int Id)
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
