using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries
{
    public class ObtenerLimiteExistenciaQueryHandler : IRequestHandler<ObtenerLimiteExistenciaQuery, ObtenerModeloLimiteExistencia>
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public ObtenerLimiteExistenciaQueryHandler(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloLimiteExistencia> Handle(ObtenerLimiteExistenciaQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloLimiteExistencia>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
