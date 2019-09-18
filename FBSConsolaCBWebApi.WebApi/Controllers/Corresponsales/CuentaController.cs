using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CuentaController : Controller
    {
        private readonly IMediator _mediador;

        public CuentaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("lista", Name = "Cuenta_ListarCuentaSegunIdentificacion")]
        public ActionResult<ListaCuentaMS> List([FromBody] ListaCuentaME modelo)
        {
            return new ListaCuentaMS()
            {
                Cuentas = new List<ModeloListaCuentas>() {
                new ModeloListaCuentas() {
                    Tipo = "Débito",
                    NumeroCuenta = "34836943492303",
                    SaldoActual = 3546.56
            },new ModeloListaCuentas() {
                    Tipo = "Crédito",
                    NumeroCuenta = "7483693492303",
                    SaldoActual = 6598.03
            } }
            };
        }
    }
}
