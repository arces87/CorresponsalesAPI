using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerDispositivoHandler : IRequestHandler<ObtenerDispositivoME, ObtenerDispositivoMS>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerDispositivoHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerDispositivoMS> Handle(ObtenerDispositivoME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerDispositivoMS>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
