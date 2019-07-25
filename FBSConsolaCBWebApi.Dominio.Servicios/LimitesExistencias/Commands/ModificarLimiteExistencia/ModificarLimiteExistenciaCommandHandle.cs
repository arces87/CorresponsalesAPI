using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands
{
    public class ModificarLimiteExistenciaCommandHandle : IRequestHandler<ModificarLimiteExistenciaCommand, int>
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public ModificarLimiteExistenciaCommandHandle(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarLimiteExistenciaCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetWithAssociations(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
