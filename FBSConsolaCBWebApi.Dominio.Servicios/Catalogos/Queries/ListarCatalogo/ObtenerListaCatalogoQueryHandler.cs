using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerListaCatalogoQueryHandler : IRequestHandler<ObtenerListaCatalogoQuery, ModeloObtenerListaCatalogo>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaCatalogoQueryHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaCatalogo> Handle(ObtenerListaCatalogoQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            var _retorno = new ModeloObtenerListaCatalogo();
            _retorno.TotalElementos = _model.Count();
            Filtro<Catalogo>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Catalogos = _mapper.Map<List<ModeloObtenerDetalleListaCatalogo>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
