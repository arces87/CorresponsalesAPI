using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class CambioContraseniaME : IRequest<string>
    {
        public string Token { get; set; }
        public string Contrasenia { get; set; }

    }
}
