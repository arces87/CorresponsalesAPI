using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBS.Dominio.Servicios.GestionFicheros;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GestionFicheroController : Controller
    {
        private readonly IMediator _mediador;

        public GestionFicheroController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("guardarFichero", Name = "GestionFichero_GuardarFichero")]
        public async Task<ActionResult<string>> List([FromForm] GuardarFicheroME ficheroSalvar)
        {
            var nombreDirectorio = Path.Combine("Resources", "Imagenes");
            var direccionSalvar = Path.Combine(Directory.GetCurrentDirectory(), nombreDirectorio);
            ficheroSalvar.DireccionGuardar = direccionSalvar;
            return await _mediador.Send(ficheroSalvar);
        }
    }
}
