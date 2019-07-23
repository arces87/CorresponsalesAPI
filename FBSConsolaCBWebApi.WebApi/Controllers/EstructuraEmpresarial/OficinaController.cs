using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class OficinaController : Controller
    {
        private readonly IMediator _mediador;

        public OficinaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet]
        public async Task<ActionResult<ModeloObtenerListaOficina>> Get()
        {
            return await _mediador.Send(new ObtenerListaOficinaQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ObtenerModeloOficina>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerOficinaQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearOficinaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarOficinaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarOficinaCommand() { Id = Id });
        }
    }
}
