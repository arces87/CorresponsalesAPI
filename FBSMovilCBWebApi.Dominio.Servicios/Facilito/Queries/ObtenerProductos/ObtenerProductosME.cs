using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerProductosME : IRequest<ObtenerProductosMS>
    {
        public string IdGrupo { get; set; }
        public string Servicio { get; set; }
    }
}
