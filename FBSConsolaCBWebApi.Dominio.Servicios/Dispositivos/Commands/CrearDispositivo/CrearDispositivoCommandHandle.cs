using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class CrearDispositivoCommandHandle : IRequestHandler<CrearDispositivoCommand, int>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public CrearDispositivoCommandHandle(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearDispositivoCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Dispositivo>(request);
            await _repositorio.Add(_model);
            return 0;
        }
    }
}
