using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class DatosValidarOTPME : IRequest<ProcesarValidarOtpMS>
    {
        public string Usuario { get; set; }
        public string Otp { get; set; }
        public int NumeroIntento { get; set; }
    }
}
