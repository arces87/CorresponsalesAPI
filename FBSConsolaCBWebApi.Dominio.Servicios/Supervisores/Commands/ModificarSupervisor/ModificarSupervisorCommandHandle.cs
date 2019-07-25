using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class ModificarSupervisorCommandHandle : IRequestHandler<ModificarSupervisorCommand, int>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ModificarSupervisorCommandHandle(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarSupervisorCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetWithAssociations(request.Id) as Supervisor;
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
