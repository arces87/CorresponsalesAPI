using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries
{
    public class ObtenerCorresponsalQueryHandler : IRequestHandler<ObtenerCorresponsalQuery, ObtenerModeloCorresponsal>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerCorresponsalQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloCorresponsal> Handle(ObtenerCorresponsalQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloCorresponsal>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
