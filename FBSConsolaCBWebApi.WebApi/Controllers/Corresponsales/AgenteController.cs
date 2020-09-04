using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AgenteController : Controller
    {
        private readonly IMediator _mediador;

        public AgenteController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Agente_ListarAgentes")]
        public async Task<ActionResult<ListaAgenteMS>> List([FromBody] ListaAgenteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaActivar", Name = "Agente_ListarAgentesActivar")]
        public async Task<ActionResult<ListaActivarAgenteMS>> ListaActivar([FromBody] ListaActivarAgenteME modelo)
        {
            return await _mediador.Send(modelo);
        }
        [HttpPost("listaConsola", Name = "Agente_ListarAgentesConsola")]
        public async Task<ActionResult<ListaAgenteConsolaMS>> ListaConsola([FromBody] ListaAgenteConsolaME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaUsuariosDisponibles", Name = "Agente_ListarUsuariosDisponibles")]
        public async Task<ActionResult<ListaUsuariosDiposniblesMS>> ListaUsuariosDisponibles([FromBody] ListaUsuariosDiposniblesME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaSupervisoresDisponibles", Name = "Agente_ListarSupervisoresDisponibles")]
        public async Task<ActionResult<ListaSupervisoresDiposniblesMS>> ListaSupervisoresDisponibles([FromBody] ListaSupervisoresDiposniblesME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("listaDispositivosDisponibles", Name = "Agente_ListarDispositivosDisponibles")]
        public async Task<ActionResult<ListaDispositivosDiposniblesMS>> ListarDispositivosDisponibles([FromBody] ListaDispositivosDiposniblesME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtener", Name = "Agente_ObtenerAgente")]
        public async Task<ActionResult<ObtenerAgenteMS>> Get([FromBody] ObtenerAgenteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("crear", Name = "Agente_CrearAgente")]
        public async Task<ActionResult<int>> Create([FromBody] CrearAgenteME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPut("actualizar", Name = "Agente_ActualizarAgente")]
        public async Task<ActionResult<int>> Update([FromBody] ModificarAgenteME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpDelete("eliminar", Name = "Agente_EliminarAgente")]
        public async Task<ActionResult<int>> Delete([FromBody] EliminarAgenteME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("activar", Name = "Agente_ActivarAgente")]
        public async Task<ActionResult<string>> Activar([FromBody] ActivarAgenteME modelo)
        {
            return Ok(await _mediador.Send(modelo));
        }

        [HttpPost("verificarIdentificacion", Name = "Agente_VerificarIdentificacion")]
        public async Task<ActionResult<bool>> VerificarIdentificacion([FromBody] VerificarIdentificacionAgenteME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtenerGeolocalizacion", Name = "Agente_ObtenerGeolocalizacion")]
        public async Task<ActionResult<ObtenerObtenerGeolocalizacionMS>> ObtenerGeolocalizacion([FromBody] ObtenerObtenerGeolocalizacionME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("obtenerEstadoReposicion", Name = "Agente_ObtenerEstadoReposicion")]
        public async Task<ActionResult<ObtenerEstadoReposicionAgenteMS>> ObtenerEstadoReposicion([FromBody] ObtenerEstadoReposicionAgenteME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
