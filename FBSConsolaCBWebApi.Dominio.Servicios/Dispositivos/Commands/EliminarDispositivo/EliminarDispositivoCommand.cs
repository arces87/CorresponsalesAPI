using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class EliminarDispositivoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
