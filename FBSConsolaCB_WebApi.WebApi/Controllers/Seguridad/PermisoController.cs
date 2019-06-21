using System.Collections.Generic;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Identity.Domain.Models.Seguridad;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using Microsoft.AspNetCore.Mvc;

namespace GeNe.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PermisoController : Controller
    {
        private readonly IPermisoService _service;

        public PermisoController(IPermisoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<PermisoModel> Get()
        {
            return _service.List();
        }

        [HttpPost("lista")]
        public FuenteDatosModel<PermisoModel> Get([FromBody] PaginacionModel filtro)
        {
            return _service.List(filtro);
        }
    }
}
