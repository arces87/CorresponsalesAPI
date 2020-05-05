using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ListarAlertaHandler : IRequestHandler<ListarAlertaME, ListarAlertaMS>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ListarAlertaHandler(IRepositorioAlerta repositorio, IMapper mapper, IHttpContextAccessor httpContext, IRepositorioAgente repositorioAgente)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _httpContext = httpContext;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<ListarAlertaMS> Handle(ListarAlertaME request, CancellationToken cancellationToken)
        {
           
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var model = await _repositorio.GetForAgente(agente.Id.ToString());
            var retorno = new ListarAlertaMS();
            retorno.Alertas = _mapper.Map<List<ModeloListaAlerta>>(model.OrderByDescending(a => a.Fecha).Take(request.CantidadElementos));
            return retorno;
        }
    }
}
