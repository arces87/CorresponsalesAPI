using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DispositivoController : Controller
    {
        private readonly IMediator _mediador;

        public DispositivoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Dispositivo_ListarDispositivos")]
        public async Task<ActionResult<ListaDispositivoMS>> List([FromBody] ListaDispositivoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Dispositivo_ObtenerDispositivo")]
        public async Task<ActionResult<ObtenerDispositivoMS>> Get([FromBody] ObtenerDispositivoME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("crear", Name = "Dispositivo_CrearDispositivo")]
        public async Task<ActionResult<int>> Create([FromBody] CrearDispositivoME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPut("actualizar", Name = "Dispositivo_ActualizarDispositivo")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarDispositivoME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("eliminar", Name = "Dispositivo_EliminarDispositivo")]
        public async Task<ActionResult<int>> Delete([FromBody] EliminarDispositivoME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("verificarDispositivo", Name = "Dispositivo_VerificarDispositivo")]
        public async Task<ActionResult<bool>> VerificarDispositivo([FromBody] VerificarDispositivoME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
