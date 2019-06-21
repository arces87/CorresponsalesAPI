using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBS_Core.Base.Domain.Models.Filtro;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCB_WebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TipoCatalogoController : Controller
    {
        private readonly ITipoCatalogoService _service;

        public TipoCatalogoController(ITipoCatalogoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<TipoCatalogoModel> Get()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<TipoCatalogoModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }

        [HttpGet("{id}")]
        public TipoCatalogoModel Get(int Id)
        {
            return _service.Get(Id);
        }

        [HttpPut]
        public async Task<object> Update([FromBody] TipoCatalogoModel model)
        {
            var result = await _service.Update(model);

            if (result != null)
            {
                return result;
            }

            throw new ApplicationException("INVALID_DATA_ATTEMPT");
        }
    }
}
