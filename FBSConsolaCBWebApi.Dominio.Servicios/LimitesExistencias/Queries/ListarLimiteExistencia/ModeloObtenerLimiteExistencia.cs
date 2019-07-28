using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries
{
    public class ModeloObtenerLimiteExistencia
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaLimiteExistencia> Limites { get; set; }
    }
}
