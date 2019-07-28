using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries
{
    public class ModeloObtenerListaEmpresa
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloObtenerDetalleListaEmpresa> Empresas { get; set; }
    }
}
