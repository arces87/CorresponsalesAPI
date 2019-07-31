using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries
{
    public class ObtenerSupervisorQueryHandler : IRequestHandler<ObtenerSupervisorQuery, ObtenerModeloSupervisor>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerSupervisorQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloSupervisor> Handle(ObtenerSupervisorQuery request, CancellationToken cancellationToken)
        {
            var supervisor = _mapper.Map<ObtenerModeloSupervisor>(await _repositorio.GetWithAssociations(request.Id));
            return supervisor;
        }
    }
}
