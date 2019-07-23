using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerDispositivoQueryHandler : IRequestHandler<ObtenerDispositivoQuery, ObtenerModeloDispositivo>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerDispositivoQueryHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloDispositivo> Handle(ObtenerDispositivoQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloDispositivo>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
