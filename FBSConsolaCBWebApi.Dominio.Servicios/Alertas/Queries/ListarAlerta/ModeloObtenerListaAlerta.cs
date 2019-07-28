using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ModeloObtenerListaAlerta
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaAlerta> Alertas { get; set; }
    }
}
