using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ObtenerUsuarioME : IRequest<ModeloObtenerUsuario>
    {
        public string Id { get; set; }
    }
}