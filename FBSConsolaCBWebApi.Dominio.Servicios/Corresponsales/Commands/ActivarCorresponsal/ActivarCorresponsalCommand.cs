using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class ActivarCorresponsalCommand : INotification
    {
        public int IdCorresponsal { get; set; }
    }
}
