using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Menus.Queries
{
    public class ObtenerMenuME : IRequest<ObtenerMenuMS>
    {
        public string Id { get; set; }
    }
}