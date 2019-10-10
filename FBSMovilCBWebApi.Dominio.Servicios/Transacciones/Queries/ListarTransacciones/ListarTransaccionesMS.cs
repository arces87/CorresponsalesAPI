using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionesMS
    {
        public IEnumerable<ModeloTransaccion> Transacciones { get; set; }
    }
}
