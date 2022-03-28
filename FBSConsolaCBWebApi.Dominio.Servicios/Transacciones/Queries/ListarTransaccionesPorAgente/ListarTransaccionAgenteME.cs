using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionAgenteME : IRequest<ListarTransaccionAgenteMS>
    {

        public string IdAgente { get; set; }
        public string IdTipoTransaccion { get; set; }
    }
}