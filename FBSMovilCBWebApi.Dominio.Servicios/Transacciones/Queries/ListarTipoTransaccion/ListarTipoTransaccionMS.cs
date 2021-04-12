using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTipoTransaccionMS
    {
        public double SaldoCaja { get; set; }
        public List<ModeloListaTipoTransaccion> TiposTransacciones { get; set; }
    }
}
