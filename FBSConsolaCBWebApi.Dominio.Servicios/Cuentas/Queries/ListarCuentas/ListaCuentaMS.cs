using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListaCuentaMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListaCuentas> Cuentas { get; set; }
    }
}
