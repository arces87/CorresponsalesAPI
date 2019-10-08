using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class CerrarDiaME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public string Mac { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
