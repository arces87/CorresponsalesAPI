using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ModeloObtenerLimiteTransaccional
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaLimiteTransaccional> Limites { get; set; }
    }
}
