using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerComisionPorTipoTransaccionME : IRequest<ObtenerComisionPorTipoTransaccionMS>
    {
        public string IdAgente { get; set; }
        public string IdTipoTransaccion { get; set; }
    }
}