using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corresponsales.Command.Model;
using Corresponsales.Query.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBSMovilCBWebApi.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion("1.0")]
    public class PrestamoController : Controller
    {
        private readonly IMediator _mediador;

        public PrestamoController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost("listarPrestamos", Name = "Prestamos_Listar")]
        [Produces(typeof(DevuelveInformacionDePrestamosResponse))]
        public async Task<ActionResult<DevuelveInformacionDePrestamosResponse>> ListarPrestamos([FromBody] ListarPrestamosME modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPost("efectivizarPrestamos", Name = "Prestamos_Efectivizar")]
        [Produces(typeof(EfectivizacionPrestamoResponse))]
        public async Task<ActionResult<EfectivizacionPrestamoResponse>> EfectivizarPrestamos([FromBody] ProcesarAbonoPrestamoME modelo)
        {
            return await _mediador.Send(modelo);
        }
    }
}