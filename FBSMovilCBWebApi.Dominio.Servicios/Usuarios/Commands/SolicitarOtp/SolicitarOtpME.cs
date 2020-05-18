using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarOtpME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Identificacion { get; set; }
        public bool ParaAgente { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
