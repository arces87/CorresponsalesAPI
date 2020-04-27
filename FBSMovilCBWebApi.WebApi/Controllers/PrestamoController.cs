using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PrestamoController : Controller
    {
        private readonly IMediator _mediador;

        public PrestamoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("listarPrestamos", Name = "Prestamos_Listar")]
        [Produces(typeof(InformacionPrestamosMSL))]
        public async Task<ActionResult<InformacionPrestamosMSL>> ListarPrestamos([FromBody] ListarPrestamosME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("efectivizarPrestamos", Name = "Prestamos_Efectivizar")]
        [Produces(typeof(EfectivizacionPrestamoMS))]
        public async Task<ActionResult<EfectivizacionPrestamoMS>> EfectivizarPrestamos([FromBody] ProcesarAbonoPrestamoME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}