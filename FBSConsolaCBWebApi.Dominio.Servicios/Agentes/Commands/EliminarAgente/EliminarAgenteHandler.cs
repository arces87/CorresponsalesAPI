using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class EliminarAgenteHandler : IRequestHandler<EliminarAgenteME, bool>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IMapper _mapper;

        public EliminarAgenteHandler(IRepositorioAgente repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarAgenteME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Agente>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
