using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class ComprobarUsuarioME : IRequest<bool>
    {
        public string Usuario { get; set; }

    }
}
