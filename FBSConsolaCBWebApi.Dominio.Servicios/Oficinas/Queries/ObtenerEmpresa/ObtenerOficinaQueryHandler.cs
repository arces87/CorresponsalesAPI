using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries
{
    public class ObtenerOficinaQueryHandler : IRequestHandler<ObtenerOficinaQuery, ObtenerModeloOficina>
    {
        private readonly IRepositorioOficina _repositorio;
        private readonly IMapper _mapper;

        public ObtenerOficinaQueryHandler(IRepositorioOficina repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloOficina> Handle(ObtenerOficinaQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloOficina>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
