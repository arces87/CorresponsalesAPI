using MediatR;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class CrearAlertaCommand : IRequest<int>
    {
        public int IdConversacion { get; set; }

        public int IdDestinatario { get; set; }

        public int IdRemitente { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public int Estado { get; set; }
    }
}
