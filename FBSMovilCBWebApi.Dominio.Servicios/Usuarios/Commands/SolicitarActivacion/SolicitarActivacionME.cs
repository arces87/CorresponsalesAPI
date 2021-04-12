using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarActivacionME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public string Imei { get; set; }
        public string Mac { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
