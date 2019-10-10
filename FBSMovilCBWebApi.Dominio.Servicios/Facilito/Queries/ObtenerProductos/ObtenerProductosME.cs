using MediatR;
using ServiciosFacilito.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerProductosME : IRequest<ObtenerProductosFacilitoResponse>
    {
        public string IdGrupo { get; set; }
        public string Servicio { get; set; }
    }
}
