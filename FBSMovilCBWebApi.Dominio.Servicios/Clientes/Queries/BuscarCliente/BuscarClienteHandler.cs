using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteHandler : IRequestHandler<BuscarClienteME, BuscarClienteMS>
    {

        public BuscarClienteHandler()
        {
        }

        public async Task<BuscarClienteMS> Handle(BuscarClienteME request, CancellationToken cancellationToken)
        {
            return new BuscarClienteMS() { NumeroCliente = "1234", Identificacion = "3245334345", NombreCompleto = "Juan Pedro Ramirez Perez", SecuencialCliente = "14324" };
        }
    }
}
