using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Roles.Queries
{
    public class ListaRolMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListaRol> Roles { get; set; }
    }
}
