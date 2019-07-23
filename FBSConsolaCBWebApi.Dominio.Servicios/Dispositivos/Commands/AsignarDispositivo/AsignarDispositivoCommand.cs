using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class AsignarDispositivoCommand : INotification
    {
        public int IdCorresponsal { get; set; }
        public int IdDispositivo { get; set; }
    }
}
