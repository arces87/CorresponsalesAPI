using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class DesasignarDispositivoCommandHandle : INotificationHandler<DesasignarDispositivoCommand>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public DesasignarDispositivoCommandHandle(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task Handle(DesasignarDispositivoCommand request, CancellationToken cancellationToken)
        {
            await _repositorio.Desasignar(request.IdDispositivo, request.IdCorresponsal);
        }
    }
}
