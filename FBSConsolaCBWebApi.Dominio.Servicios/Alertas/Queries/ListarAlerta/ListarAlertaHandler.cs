using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
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
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ListarAlertaMS();
            var totalElementos = 0;
            Filtro<Alerta>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Alertas = _mapper.Map<List<ModeloListaAlerta>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
