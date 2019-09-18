using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaUsuariosDiposniblesMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListaUsuariosDiposnibles> Usuarios { get; set; }
    }
}
