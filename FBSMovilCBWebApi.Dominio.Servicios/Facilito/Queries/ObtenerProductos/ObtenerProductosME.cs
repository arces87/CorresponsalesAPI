using MediatR;
using ServiciosFacilito.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ObtenerProductosME : IRequest<ObtenerProductosFacilitoMS>
    {
        public string IdGrupo { get; set; }
        public string Servicio { get; set; }
    }
}
