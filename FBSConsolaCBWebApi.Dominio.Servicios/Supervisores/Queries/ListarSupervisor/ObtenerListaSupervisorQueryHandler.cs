using AutoMapper;
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
            var _model = await _repositorio.GetAllWithAssociations();
            var _personas = _mapper.Map<List<ModeloObtenerDetalleListaSupervisor>>(_model);
            return new ModeloObtenerListaSupervisor() { Personas = _personas };
        }
    }
}
