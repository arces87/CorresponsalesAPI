using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class VerificarUsuarioME : IRequest<bool>
    {
        public string Codigo { get; set; }
        public string IdUsuario { get; set; }
    }
}