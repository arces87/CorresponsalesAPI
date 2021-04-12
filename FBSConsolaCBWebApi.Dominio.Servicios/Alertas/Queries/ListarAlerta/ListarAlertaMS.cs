using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ListarAlertaMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListaAlerta> Alertas { get; set; }
    }
}
