using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ModeloObtenerListaPersona
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaPersona> Personas { get; set; }
    }
}
