using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ObtenerTipoCatalogoQueryHandler : IRequestHandler<ObtenerTipoCatalogoQuery, ObtenerModeloTipoCatalogo>
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerTipoCatalogoQueryHandler(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloTipoCatalogo> Handle(ObtenerTipoCatalogoQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloTipoCatalogo>(await _repositorio.Get(request.Id));
        }
    }
}
