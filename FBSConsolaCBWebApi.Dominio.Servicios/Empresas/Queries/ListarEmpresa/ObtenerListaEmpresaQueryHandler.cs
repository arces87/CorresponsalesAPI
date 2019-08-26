using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries
{
    public class ObtenerListaEmpresaQueryHandler : IRequestHandler<ObtenerListaEmpresaQuery, ModeloObtenerListaEmpresa>
    {
        private readonly IRepositorioEmpresa _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaEmpresaQueryHandler(IRepositorioEmpresa repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaEmpresa> Handle(ObtenerListaEmpresaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            var _retorno = new ModeloObtenerListaEmpresa();
            var totalElementos = 0;
            Filtro<Empresa>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Empresas = _mapper.Map<List<ModeloObtenerDetalleListaEmpresa>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
