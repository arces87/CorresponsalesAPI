using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class DatosOtpME : IRequest<ProcesarOtpMS>
    {
        public string Usuario { get; set; }
    }
}
