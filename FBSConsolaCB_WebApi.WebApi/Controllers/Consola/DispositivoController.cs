using System;
using System.Collections.Generic;
using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.Consola;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Consola;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DispositivoController : Controller
    {
        private readonly IDispositivoService _service;

        public DispositivoController(IDispositivoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<DispositivoModel> List()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<DispositivoModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("get/{id}")]
        public DispositivoModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPost]
        public DispositivoModel Create([FromBody] DispositivoModel model)
        {
            var result = _service.Create(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpPut]
        public DispositivoModel Update([FromBody] DispositivoModel model)
        {
            var result = _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }

        [HttpDelete("{id}")]
        public DispositivoModel Delete(int Id)
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
