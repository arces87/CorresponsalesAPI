using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands
{
    public class CrearLimiteTransaccionalCommandHandle : IRequestHandler<CrearLimiteTransaccionalCommand, int>
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public CrearLimiteTransaccionalCommandHandle(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearLimiteTransaccionalCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<LimiteTransaccional>(request);
            await _repositorio.Add(_model);
            return 0;
        }
    }
}
