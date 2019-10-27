using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class VerificarIdentificacionAgenteHandler : IRequestHandler<VerificarIdentificacionAgenteME, bool>
    {
        private readonly IRepositorioAgente _repositorio;

        public VerificarIdentificacionAgenteHandler(IRepositorioAgente repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> Handle(VerificarIdentificacionAgenteME request, CancellationToken cancellationToken)
        {
            return await _repositorio.Verificaridentificacion(request.Identificacion);
        }
    }
}
