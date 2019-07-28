using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries
{
    public class ObtenerListaOficinaQueryHandler : IRequestHandler<ObtenerListaOficinaQuery, ModeloObtenerListaOficina>
    {
        private readonly IRepositorioOficina _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaOficinaQueryHandler(IRepositorioOficina repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaOficina> Handle(ObtenerListaOficinaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ModeloObtenerListaOficina();
            _retorno.TotalElementos = _model.Count();
            Filtro<Oficina>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Oficinas = _mapper.Map<List<ModeloObtenerDetalleListaOficina>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
