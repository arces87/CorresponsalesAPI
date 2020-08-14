using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
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
        private readonly IMediator _mediador;

        public ListarAlertaHandler(
            IRepositorioAlerta repositorio, 
            IMapper mapper, 
            IHttpContextAccessor httpContext, 
            IRepositorioAgente repositorioAgente,
            IMediator mediador)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _httpContext = httpContext;
            _repositorioAgente = repositorioAgente;
            _mediador = mediador;
        }

        public async Task<ListarAlertaMS> Handle(ListarAlertaME request, CancellationToken cancellationToken)
        {

            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var model = await _repositorio.GetForAgente(agente.Id.ToString());
            var retorno = new ListarAlertaMS();
            retorno.Alertas = _mapper.Map<List<ModeloListaAlerta>>(model.OrderByDescending(a => a.Fecha).Take(request.CantidadElementos));
            return retorno;
        }
    }
}
