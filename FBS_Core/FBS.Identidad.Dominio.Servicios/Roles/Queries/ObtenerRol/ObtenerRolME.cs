using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Roles.Queries
{
    public class ObtenerRolME : IRequest<ModeloObtenerRol>
    {
        public string Id { get; set; }
    }
}