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

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries
{
    public class ObtenerListaLimiteExistenciaQueryHandler : IRequestHandler<ObtenerListaLimiteExistenciaQuery, ModeloObtenerLimiteExistencia>
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaLimiteExistenciaQueryHandler(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerLimiteExistencia> Handle(ObtenerListaLimiteExistenciaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            var _retorno = new ModeloObtenerLimiteExistencia();
            _retorno.TotalElementos = _model.Count();
            Filtro<LimiteExistencia>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Limites = _mapper.Map<List<ModeloObtenerDetalleListaLimiteExistencia>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
