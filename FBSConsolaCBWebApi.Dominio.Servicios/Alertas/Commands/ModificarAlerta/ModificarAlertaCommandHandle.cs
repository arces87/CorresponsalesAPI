using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class ModificarAlertaCommandHandle : IRequestHandler<ModificarAlertaCommand, int>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public ModificarAlertaCommandHandle(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(ModificarAlertaCommand request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id;
        }
    }
}
