using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaSupervisoresDiposniblesMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListaSupervisoresDiposnibles> Supervisores { get; set; }
    }
}
