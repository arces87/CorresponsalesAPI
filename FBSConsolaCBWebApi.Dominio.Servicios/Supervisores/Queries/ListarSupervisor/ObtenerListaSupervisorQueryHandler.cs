using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries
{
    public class ObtenerListaSupervisorQueryHandler : IRequestHandler<ObtenerListaSupervisorQuery, ModeloObtenerListaSupervisor>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaSupervisorQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaSupervisor> Handle(ObtenerListaSupervisorQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetSupervisoresWithAssociations();
            var _retorno = new ModeloObtenerListaSupervisor();
            var totalElementos = 0;
            Filtro<Supervisor>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Personas = _mapper.Map<List<ModeloObtenerDetalleListaSupervisor>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
