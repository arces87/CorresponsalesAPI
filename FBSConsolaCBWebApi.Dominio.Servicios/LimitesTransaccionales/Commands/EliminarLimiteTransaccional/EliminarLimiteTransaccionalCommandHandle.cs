using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands
{
    public class EliminarLimiteTransaccionalCommandHandle : IRequestHandler<EliminarLimiteTransaccionalCommand, bool>
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public EliminarLimiteTransaccionalCommandHandle(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarLimiteTransaccionalCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<LimiteTransaccional>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
