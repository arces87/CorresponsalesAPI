using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerCatalogoHandler : IRequestHandler<ObtenerCatalogoME, ObtenerCatalogoMS>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerCatalogoHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerCatalogoMS> Handle(ObtenerCatalogoME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerCatalogoMS>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
