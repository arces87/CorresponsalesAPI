using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Menus.Commands
{
    public class EliminarMenuME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
