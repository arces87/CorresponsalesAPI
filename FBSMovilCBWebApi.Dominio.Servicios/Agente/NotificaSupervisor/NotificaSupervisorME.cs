using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class NotificaSupervisorME : INotification
    {
        public string MensajeExcepcion { get; set; }

        public string IdUsuario { get; set; }
    }
}