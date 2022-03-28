using MediatR;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class CrearRolME : IRequest<string>
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public IEnumerable<CrearRolMenu> Menus { get; set; }
    }
}
