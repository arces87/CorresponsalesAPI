using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Menus.Commands
{
    public class CrearMenuME : IRequest<string>
    {
        public string Nombre { get; set; }

        public int Orden { get; set; }

        public bool EstaActivo { get; set; }

        public string Icono { get; set; }

        public string Ruta { get; set; }

        public string MenuId { get; set; }
    }
}
