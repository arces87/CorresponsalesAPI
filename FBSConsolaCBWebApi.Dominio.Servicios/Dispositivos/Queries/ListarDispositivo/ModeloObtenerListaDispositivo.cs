using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ModeloObtenerListaDispositivo
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }

        public List<ModeloObtenerDetalleListaDispositivo> Dispositivos { get; set; }
    }
}
