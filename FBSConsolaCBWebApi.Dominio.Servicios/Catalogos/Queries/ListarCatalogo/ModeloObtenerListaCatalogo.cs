using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ModeloObtenerListaCatalogo
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaCatalogo> Catalogos { get; set; }
    }
}
