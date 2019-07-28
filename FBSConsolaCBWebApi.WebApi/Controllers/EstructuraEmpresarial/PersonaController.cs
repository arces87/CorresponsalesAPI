using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Servicios.Personas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PersonaController : Controller
    {
        private readonly IMediator _mediador;

        public PersonaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Persona_ListarPersonas")]
        public async Task<ActionResult<ModeloObtenerListaPersona>> Get([FromBody] ObtenerListaPersonaQuery modelo)
        {
            return await _mediador.Send(modelo);
        }


        [HttpGet("{id}", Name = "Persona_ObtenerPersona")]
        public async Task<ActionResult<ObtenerModeloPersona>> Get(ObtenerPersonaQuery modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpGet("DevuelvePersonaIdentificacion", Name = "Persona_ObtenerPersonaIdentificacion")]
        public async Task<ActionResult<ObtenerModeloPersonaIdentificacion>> DevuelveDatosPersonaIdentificacion(string identificacion)
        {
            return await _mediador.Send(new ObtenerPersonaIdentificacionQuery() { Identificacion = identificacion });
        }

        [HttpDelete("{id}", Name = "Persona_EliminarPersona")]
        public async Task<ActionResult<bool>> Delete(EliminarPersonaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
