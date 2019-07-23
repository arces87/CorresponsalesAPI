using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerLogQueryHandler : IRequestHandler<ObtenerLogQuery, ObtenerModeloLog>
    {
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;

        public ObtenerLogQueryHandler(IRepositorioLog repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloLog> Handle(ObtenerLogQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloLog>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
