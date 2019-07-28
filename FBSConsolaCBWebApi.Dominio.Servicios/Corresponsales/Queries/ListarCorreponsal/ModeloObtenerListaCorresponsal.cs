using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries
{
    public class ModeloObtenerListaCorresponsal
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaCorresponsal> Personas { get; set; }
    }
}
