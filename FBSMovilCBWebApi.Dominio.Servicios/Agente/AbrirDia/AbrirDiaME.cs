using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class AbrirDiaME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
