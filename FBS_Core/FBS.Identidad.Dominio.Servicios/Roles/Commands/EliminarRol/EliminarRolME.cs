using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class EliminarRolME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
