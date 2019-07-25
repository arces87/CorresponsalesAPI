using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands
{
    public class ModificarLimiteTransaccionalCommandHandle : IRequestHandler<ModificarLimiteTransaccionalCommand, int>
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public ModificarLimiteTransaccionalCommandHandle(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarLimiteTransaccionalCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetWithAssociations(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
