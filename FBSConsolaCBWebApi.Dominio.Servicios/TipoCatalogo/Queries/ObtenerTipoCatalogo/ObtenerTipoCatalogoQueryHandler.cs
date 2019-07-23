using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerTipoCatalogoQueryHandler : IRequestHandler<ObtenerDispositivoQuery, ObtenerModeloDispositivo>
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerTipoCatalogoQueryHandler(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloDispositivo> Handle(ObtenerDispositivoQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloDispositivo>(await _repositorio.Get(request.Id));
        }
    }
}
