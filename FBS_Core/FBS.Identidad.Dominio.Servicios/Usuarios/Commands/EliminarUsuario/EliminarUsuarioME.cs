using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class EliminarUsuarioME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
