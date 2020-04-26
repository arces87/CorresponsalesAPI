using MediatR;
using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.Notificaciones
{
    public class NotificacionME : INotification
    {
        public string PlantillaSMS { get; set; }
        public string PlantillaCorreoElectronico { get; set; }
        public string NombreDestinatario { get; set; }
        public string CorreoElectronicoDestinatario { get; set; }
        public string AsuntoCorreoElectronico { get; set; }
        public int? NumeroCliente { get; set; }
        public int? SecuencialEmpresa { get; set; }
        public Dictionary<string, string> Valores { get; set; }
    }
}