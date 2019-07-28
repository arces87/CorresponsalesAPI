using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries
{
    public class ModeloObtenerListaSupervisor
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaSupervisor> Personas { get; set; }
    }
}
