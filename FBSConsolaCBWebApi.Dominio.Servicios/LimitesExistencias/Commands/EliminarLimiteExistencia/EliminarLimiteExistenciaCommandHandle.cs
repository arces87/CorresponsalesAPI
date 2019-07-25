using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands
{
    public class EliminarLimiteExistenciaCommandHandle : IRequestHandler<EliminarLimiteExistenciaCommand, bool>
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public EliminarLimiteExistenciaCommandHandle(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarLimiteExistenciaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<LimiteExistencia>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
