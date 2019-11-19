using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Queries
{
    public class ObtenerCanalHandler : IRequestHandler<ObtenerCanalME, ObtenerCanalMS>
    {
        private readonly IRepositorioCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerCanalHandler(IRepositorioCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerCanalMS> Handle(ObtenerCanalME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerCanalMS>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
