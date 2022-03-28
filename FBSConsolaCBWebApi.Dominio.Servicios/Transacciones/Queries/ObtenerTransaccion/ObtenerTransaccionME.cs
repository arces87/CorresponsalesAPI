using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerTransaccionME : IRequest<ObtenerTransaccionMS>
    {
        public string Id { get; set; }
    }
}