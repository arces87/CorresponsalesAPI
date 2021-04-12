using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Menus.Queries
{
    public class ListaMenuUsuarioME : IRequest<ListaMenuUsuarioMS>
    {
        public string IdUsuario { get; set; }
    }
}