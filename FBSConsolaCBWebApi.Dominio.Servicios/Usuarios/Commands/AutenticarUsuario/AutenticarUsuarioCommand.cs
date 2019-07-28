using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioCommand : IRequest<ModeloAutenticacion>
    {
        public string Usuario { get; set; }
        public string Contrasenna { get; set; }

    }
}
