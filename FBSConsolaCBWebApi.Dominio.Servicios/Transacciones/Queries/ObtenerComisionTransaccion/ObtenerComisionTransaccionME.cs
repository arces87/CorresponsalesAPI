using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerComisionTransaccionME : IRequest<ObtenerComisionTransaccionMS>
    {
        public string IdAgente { get; set; }
    }
}