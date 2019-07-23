using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerCatalogoQueryHandler : IRequestHandler<ObtenerCatalogoQuery, ObtenerModeloCatalogo>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerCatalogoQueryHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloCatalogo> Handle(ObtenerCatalogoQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloCatalogo>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
