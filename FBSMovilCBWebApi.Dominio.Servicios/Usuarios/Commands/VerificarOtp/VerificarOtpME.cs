using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class VerificarOtpME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Identificacion { get; set; }
        public string Otp { get; set; }
    }
}
