using MediatR;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Menus.Commands
{
    public class ModificarMenuME : IRequest<string>
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public int Orden { get; set; }

        public string Icono { get; set; }

        public string Ruta { get; set; }

        public string MenuId { get; set; }

        public IEnumerable<ModificarMenuME> Menus { get; set; }
    }
}
