using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ModeloObtenerListaLog
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaLog> Logs { get; set; }
    }
}
