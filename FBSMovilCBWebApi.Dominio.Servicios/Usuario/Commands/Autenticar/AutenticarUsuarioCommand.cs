using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class AutenticarUsuarioCommand : IRequest<ModeloUsuarioAutenticadoMovil>
    {
        public string Usuario { get; set; }
        public string Contrasenna { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
    }
}
