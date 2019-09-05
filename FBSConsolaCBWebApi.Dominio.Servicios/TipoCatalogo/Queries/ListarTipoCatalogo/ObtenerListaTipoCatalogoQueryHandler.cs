using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.DAL.Nomenclador;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ObtenerListaTipoCatalogoQueryHandler : IRequestHandler<ObtenerListaTipoCatalogoQuery, ModeloObtenerListaTipoCatalogo>
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaTipoCatalogoQueryHandler(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaTipoCatalogo> Handle(ObtenerListaTipoCatalogoQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            var _retorno = new ModeloObtenerListaTipoCatalogo();
            var totalElementos = 0;
            Filtro<TipoCatalogo>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.TiposCatalogos = _mapper.Map<List<ModeloObtenerDetalleListaTipoCatalogo>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
