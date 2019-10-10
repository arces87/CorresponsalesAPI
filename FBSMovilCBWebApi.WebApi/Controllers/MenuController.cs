using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSMovilCBWebApi.WebApi
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IMediator _mediador;

        public MenuController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("abrirDia", Name = "Menu_AbrirDia")]
        public async Task<ActionResult<bool>> AbrirDia([FromBody] AbrirDiaME modelo)
        {
            await _mediador.Send(modelo);
            return true;
        }
    }
}
