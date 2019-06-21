using System;
using System.Collections.Generic;
using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CatalogoController : Controller
    {
        private readonly ICatalogoService _service;

        public CatalogoController(ICatalogoService service)
        {
            _service = service;
        }

        [HttpGet("{tipo}")]
        public IEnumerable<CatalogoModel> List(int Tipo)
        {
            return _service.List(Tipo);
        }

        [HttpPost("lista")]
        public FuenteDatosModel<CatalogoModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("get/{id}")]
        public CatalogoModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public CatalogoModel Create([FromBody] CatalogoModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public CatalogoModel Update([FromBody] CatalogoModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public CatalogoModel Delete(int Id)
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
