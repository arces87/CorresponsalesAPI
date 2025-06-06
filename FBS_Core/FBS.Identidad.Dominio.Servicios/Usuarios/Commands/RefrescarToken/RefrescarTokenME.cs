using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class RefrescarTokenME : IRequest<string>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
