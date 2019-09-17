using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class CrearAgenteHandler : IRequestHandler<CrearAgenteME, string>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IMapper _mapper;

        public CrearAgenteHandler(IRepositorioAgente repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearAgenteME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Agente>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
