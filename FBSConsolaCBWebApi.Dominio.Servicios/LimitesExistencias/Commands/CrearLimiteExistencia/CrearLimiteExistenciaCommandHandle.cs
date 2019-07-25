using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands
{
    public class CrearLimiteExistenciaCommandHandle : IRequestHandler<CrearLimiteExistenciaCommand, int>
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public CrearLimiteExistenciaCommandHandle(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearLimiteExistenciaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<LimiteExistencia>(request);
            await _repositorio.Add(_model);
            return 0;
        }
    }
}
