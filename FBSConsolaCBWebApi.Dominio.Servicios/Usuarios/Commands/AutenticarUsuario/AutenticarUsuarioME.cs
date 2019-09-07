using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioME : IRequest<AutenticarUsuarioMS>
    {
        public string Usuario { get; set; }
        public string Contrasenna { get; set; }

    }
}
