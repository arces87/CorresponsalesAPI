using System.Threading.Tasks;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DispositivoController : Controller
    {
        private readonly IMediator _mediador;

        public DispositivoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Dispositivo_ListarDispositivos")]
        public async Task<ActionResult<ModeloObtenerListaDispositivo>> List([FromBody] ObtenerListaDispositivoQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Dispositivo_ObtenerDispositivo")]
        public async Task<ActionResult<ObtenerModeloDispositivo>> Get([FromBody] ObtenerDispositivoQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost(Name = "Dispositivo_CrearDispositivo")]
        public async Task<ActionResult<int>> Create([FromBody] CrearDispositivoCommand model)
        {
            return await _mediador.Send(model);
        }

        [HttpPost("asignacion", Name = "Dispositivo_AsignarDispositivo")]
        public async Task<ActionResult> Asignar([FromBody] AsignarDispositivoCommand model)
        {
            await _mediador.Publish(model);
            return Ok();
        }

        [HttpPut(Name = "Dispositivo_ActualizarDispositivo")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarDispositivoCommand model)
        {
            return Ok(await _mediador.Send(model));
        }

        [HttpDelete(Name = "Dispositivo_EliminarDispositivo")]
        public async Task<ActionResult<bool>> Delete([FromBody] EliminarDispositivoCommand modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("asignacion", Name = "Dispositivo_DesasignarDispositivo")]
        public async Task<ActionResult> Desasignar([FromBody] DesasignarDispositivoCommand model)
        {
            await _mediador.Publish(model);
            return Ok();
        }
    }
}
