using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerAlertaHandler : IRequestHandler<ObtenerAlertaME, ObtenerAlertaMS>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public ObtenerAlertaHandler(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerAlertaMS> Handle(ObtenerAlertaME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerAlertaMS>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
