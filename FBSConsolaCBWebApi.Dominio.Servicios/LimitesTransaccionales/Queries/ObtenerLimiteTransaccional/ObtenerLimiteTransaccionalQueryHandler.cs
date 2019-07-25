using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ObtenerLimiteTransaccionalQueryHandler : IRequestHandler<ObtenerLimiteTransaccionalQuery, ObtenerModeloLimiteTransaccional>
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public ObtenerLimiteTransaccionalQueryHandler(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloLimiteTransaccional> Handle(ObtenerLimiteTransaccionalQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloLimiteTransaccional>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
