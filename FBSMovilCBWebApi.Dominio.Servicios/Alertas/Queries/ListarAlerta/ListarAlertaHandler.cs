using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ListarAlertaHandler : IRequestHandler<ListarAlertaME, ListarAlertaMS>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public ListarAlertaHandler(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListarAlertaMS> Handle(ListarAlertaME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations("", "");
            var _retorno = new ListarAlertaMS();
            _retorno.Alertas = _mapper.Map<List<ModeloListaAlerta>>(_model.OrderByDescending(a => a.Fecha).Take(request.CantidadElementos));
            return _retorno;
        }
    }
}
