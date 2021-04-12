using MediatR;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class ModificarRolME : IRequest<string>
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public IEnumerable<ModificarRolMenu> Menus { get; set; }
    }
}
