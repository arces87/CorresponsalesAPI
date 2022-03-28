using MediatR;
using System.Collections.Generic;

namespace FBS.Dominio.Servicios.CorreoElectronico
{
    public class EnviarCorreoElectronicoME: INotification
    {
        public List<ModeloCuentaCorreo> DireccionesDestino { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }

        public bool IsBodyHtml { get; set; } = true;
    }
}
