using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class EliminarDispositivoCommandHandle : IRequestHandler<EliminarDispositivoCommand, bool>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public EliminarDispositivoCommandHandle(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarDispositivoCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Dispositivo>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
