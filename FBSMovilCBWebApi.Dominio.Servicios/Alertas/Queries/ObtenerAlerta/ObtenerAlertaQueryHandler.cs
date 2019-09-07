using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerAlertaQueryHandler : IRequestHandler<ObtenerAlertaQuery, ObtenerModeloAlerta>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public ObtenerAlertaQueryHandler(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerModeloAlerta> Handle(ObtenerAlertaQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloAlerta>(await _repositorio.Get(request.Id));
        }
    }
}
