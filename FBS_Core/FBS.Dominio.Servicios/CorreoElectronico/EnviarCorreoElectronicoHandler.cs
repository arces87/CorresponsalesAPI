using MailKit.Net.Smtp;
using MediatR;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Dominio.Servicios.CorreoElectronico
{
    public class EnviarCorreoElectronicoHandler : INotificationHandler<EnviarCorreoElectronicoME>
    {
        private readonly string _servidorSmtp;
        private readonly int _puertoSmtp;
        private readonly string _usuario;
        private readonly string _contrasenna;
        private readonly string _direccionCuentaRemitente;
        private readonly string _nombreCuentaRemitente;
        private readonly bool _enableSsl;

        public EnviarCorreoElectronicoHandler(IConfiguration configuracion)
        {
            var configuracionCorreo = configuracion.GetSection("ConfiguracionCorreo");
            _servidorSmtp = configuracionCorreo["ServidorSmtp"];
            _puertoSmtp = int.Parse(configuracionCorreo["PuertoSmtp"]);
            _usuario = configuracionCorreo["Usuario"];
            _contrasenna = configuracionCorreo["Password"];
            _direccionCuentaRemitente = configuracionCorreo["DireccionCuentaRemitente"];
            _nombreCuentaRemitente = configuracionCorreo["NombreCuentaRemitente"];
            _enableSsl = bool.Parse(configuracionCorreo["EnableSsl"]);
        }

        public async Task Handle(EnviarCorreoElectronicoME request, CancellationToken cancellationToken)
        {
            SendEmailWithStandar(request);
        }

        private void SendEmailWithStandar(EnviarCorreoElectronicoME request)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_direccionCuentaRemitente, _nombreCuentaRemitente);
            foreach (ModeloCuentaCorreo emailaddr in request.DireccionesDestino)
            {
                mail.To.Add(new MailAddress(emailaddr.Direccion, (emailaddr.Nombre)));
            };

            mail.Subject = request.Asunto;
            mail.Body = request.Mensaje;
            mail.IsBodyHtml = request.IsBodyHtml;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_servidorSmtp, _puertoSmtp);
            smtp.Credentials = new NetworkCredential(_usuario, _contrasenna);
            smtp.EnableSsl = _enableSsl;
            smtp.ServicePoint.MaxIdleTime = 1;
            smtp.Send(mail);
        }

        private void SendEmailWithMailKit(EnviarCorreoElectronicoME request)
        {
            var mensajeEnviar = new MimeMessage();
            mensajeEnviar.To.AddRange(request.DireccionesDestino.Select(emailaddr => new MailboxAddress(emailaddr.Nombre, emailaddr.Direccion)));
            mensajeEnviar.From.Add(new MailboxAddress(_nombreCuentaRemitente, _direccionCuentaRemitente));

            mensajeEnviar.Subject = request.Asunto;
            mensajeEnviar.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Mensaje
            };

            using (var clienteCorreo = new MailKit.Net.Smtp.SmtpClient())
            {

                clienteCorreo.Connect(_servidorSmtp, _puertoSmtp, MailKit.Security.SecureSocketOptions.Auto);

                //Remove any OAuth functionality as we won't be using it. 
                clienteCorreo.AuthenticationMechanisms.Remove("XOAUTH2");

                clienteCorreo.Authenticate(_usuario, _contrasenna);

                clienteCorreo.Send(mensajeEnviar);

                clienteCorreo.Disconnect(true);
            }
        }
    }
}
