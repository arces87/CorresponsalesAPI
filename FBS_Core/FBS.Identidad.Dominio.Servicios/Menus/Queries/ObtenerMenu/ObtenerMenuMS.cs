using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Menus.Queries
{
    public class ObtenerMenuMS
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public int Orden { get; set; }

        public string Icono { get; set; }

        public string Ruta { get; set; }

        public string MenuId { get; set; }

        public IEnumerable<ObtenerMenuMS> Menus { get; set; }
    }
}
