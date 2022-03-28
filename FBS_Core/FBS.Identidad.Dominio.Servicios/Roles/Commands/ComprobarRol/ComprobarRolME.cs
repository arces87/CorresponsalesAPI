using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class ComprobarRolME : IRequest<bool>
    {
        public string Rol { get; set; }
        public string IdRol { get; set; }

    }
}
