using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerListaLogQueryHandler : IRequestHandler<ObtenerListaLogQuery, ModeloObtenerListaLog>
    {
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaLogQueryHandler(IRepositorioLog repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaLog> Handle(ObtenerListaLogQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ModeloObtenerListaLog();
            _retorno.TotalElementos = _model.Count();
            Filtro<Log>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Logs = _mapper.Map<List<ModeloObtenerDetalleListaLog>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
