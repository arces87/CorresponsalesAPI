using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTipoTransaccionME : IRequest<ListarTipoTransaccionMS>
    {
        public string IdAgente { get; set; }
    }
}