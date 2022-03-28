using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Roles.Queries
{
    public class ModeloObtenerRol
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public IEnumerable<ObtenerRolMenu> Menus { get; set; }
    }
}
