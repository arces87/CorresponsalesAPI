using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerPersonaQueryHandler : IRequestHandler<ObtenerPersonaQuery, ObtenerModeloPersona>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerPersonaQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloPersona> Handle(ObtenerPersonaQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloPersona>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
