using ServiciosFinancial.Models;
using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries
{
    public class ObtenerDistribuidosMS
    {
        public IEnumerable<DistribuidoTipoIdentificacion> TiposIdentificaciones { get; set; }
        public IEnumerable<DistribuidoAlerta> TiposAlertas { get; set; }
    }
}
