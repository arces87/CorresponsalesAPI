using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ObtenerTipoCatalogoHandler : IRequestHandler<ObtenerTipoCatalogoME, ObtenerTipoCatalogoMS>
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerTipoCatalogoHandler(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerTipoCatalogoMS> Handle(ObtenerTipoCatalogoME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerTipoCatalogoMS>(await _repositorio.Get(request.Id));
        }
    }
}
