using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class EliminarDispositivoME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
