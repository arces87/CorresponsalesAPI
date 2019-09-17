using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerDispositivoHandler : IRequestHandler<ObtenerAgenteME, ObtenerAgenteMS>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerDispositivoHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerAgenteMS> Handle(ObtenerAgenteME request, CancellationToken cancellationToken)
        {
            var dispositivo = _mapper.Map<ObtenerAgenteMS>(await _repositorio.GetWithAssociations(request.Id));
            return dispositivo;
        }
    }
}
