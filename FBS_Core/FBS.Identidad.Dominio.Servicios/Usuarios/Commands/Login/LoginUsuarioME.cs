using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class LoginUsuarioME : IRequest<ModeloLoginUsuario>
    {
        public string Usuario { get; set; }
        public string Contrasenna { get; set; }
        public string Dispositivo { get; set; }
    }
}
