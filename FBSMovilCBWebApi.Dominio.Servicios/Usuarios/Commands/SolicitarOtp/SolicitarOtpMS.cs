using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarOtpMS
    {
        public bool OtpGenerado { get; set; }
        public bool NotificationEmailError { get; set; }
        public string NotificationEmailErrorMensaje { get; set; }

        public bool NotificationSMSError { get; set; }
        public string NotificationSMSErrorMensaje { get; set; }
    }
}
