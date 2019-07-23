using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Commands;
using FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBSConsolaCBWebApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EmpresaController : Controller
    {
        private readonly IMediator _mediador;

        public EmpresaController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet]
        public async Task<ActionResult<ModeloObtenerListaEmpresa>> Get()
        {
            return await _mediador.Send(new ObtenerListaEmpresaQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ObtenerModeloEmpresa>> Get(int Id)
        {
            return await _mediador.Send(new ObtenerEmpresaQuery() { Id = Id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CrearEmpresaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] ModificarEmpresaCommand modelo)
        {
            return await _mediador.Send(modelo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            return await _mediador.Send(new EliminarEmpresaCommand() { Id = Id });
        }
    }
}
