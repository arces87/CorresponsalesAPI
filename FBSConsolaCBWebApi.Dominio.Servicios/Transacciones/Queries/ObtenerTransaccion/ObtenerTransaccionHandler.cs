using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerTransaccionHandler : IRequestHandler<ObtenerTransaccionME, ObtenerTransaccionMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IMapper _mapper;

        public ObtenerTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerTransaccionMS> Handle(ObtenerTransaccionME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerTransaccionMS>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
