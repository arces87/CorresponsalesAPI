using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ModeloObtenerLimiteTransaccional
    {
        public List<ModeloObtenerDetalleListaLimiteTransaccional> Limites { get; set; }
    }
}
