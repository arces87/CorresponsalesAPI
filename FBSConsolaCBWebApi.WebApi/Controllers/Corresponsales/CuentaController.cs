using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries;
using System.Threading.Tasks;
using ServiciosFinancial.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CuentaController : Controller
    {
        private readonly IMediator _mediador;

        public CuentaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Cuenta_ListarCuentaSegunIdentificacion")]
        public async Task<ActionResult<ConsolidadoCuentasMSL>> List([FromBody] ListaCuentaME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}
