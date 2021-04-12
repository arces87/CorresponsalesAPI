using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ModeloListaRecaudaciones
    {
        public double Total { get; set; }
        public double Comisiones { get; set; }
        public IEnumerable<ModeloTransaccion> Lista { get; set; }
    }
}
