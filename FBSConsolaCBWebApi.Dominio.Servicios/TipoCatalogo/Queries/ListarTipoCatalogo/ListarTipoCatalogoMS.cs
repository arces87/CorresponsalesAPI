using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ListarTipoCatalogoMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListarTipoCatalogo> TiposCatalogos { get; set; }
    }
}
