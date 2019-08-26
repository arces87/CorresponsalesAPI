using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ObtenerListaLimiteTransaccionalQueryHandler : IRequestHandler<ObtenerListaLimiteTransaccionalQuery, ModeloObtenerLimiteTransaccional>
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaLimiteTransaccionalQueryHandler(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerLimiteTransaccional> Handle(ObtenerListaLimiteTransaccionalQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            var _retorno = new ModeloObtenerLimiteTransaccional();
            var totalElementos = 0;
            Filtro<LimiteTransaccional>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Limites = _mapper.Map<List<ModeloObtenerDetalleListaLimiteTransaccional>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
