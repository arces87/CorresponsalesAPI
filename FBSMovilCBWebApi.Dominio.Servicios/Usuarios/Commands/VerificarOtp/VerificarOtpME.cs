using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class VerificarOtpME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Identificacion { get; set; }
        public string Otp { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
        public string Identificaion { get; set; }
    }
}
