using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class AsignarDispositivoCommandHandle : INotificationHandler<AsignarDispositivoCommand>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public AsignarDispositivoCommandHandle(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task Handle(AsignarDispositivoCommand request, CancellationToken cancellationToken)
        {
            await _repositorio.Asignar(request.IdDispositivo, request.IdCorresponsal);
        }
    }
}
