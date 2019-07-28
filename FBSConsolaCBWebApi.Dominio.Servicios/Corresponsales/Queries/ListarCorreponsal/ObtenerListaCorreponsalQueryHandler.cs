using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries
{
    public class ObtenerListaCorreponsalQueryHandler : IRequestHandler<ObtenerListaCorresponsalQuery, ModeloObtenerListaCorresponsal>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaCorreponsalQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaCorresponsal> Handle(ObtenerListaCorresponsalQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetCorresponsalesWithAssociations();
            var _retorno = new ModeloObtenerListaCorresponsal();
            _retorno.TotalElementos = _model.Count();
            Filtro<Corresponsal>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Personas = _mapper.Map<List<ModeloObtenerDetalleListaCorresponsal>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
