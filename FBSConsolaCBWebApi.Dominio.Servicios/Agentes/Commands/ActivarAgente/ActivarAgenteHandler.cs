using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class ActivarAgenteHandler : IRequestHandler<ActivarAgenteME, string>
    {
        private readonly IRepositorioAgente _repositorio;

        public ActivarAgenteHandler(IRepositorioAgente repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<string> Handle(ActivarAgenteME request, CancellationToken cancellationToken)
        {
            await _repositorio.Activar(request.IdAgente);
            return request.IdAgente;
        }
    }
}
