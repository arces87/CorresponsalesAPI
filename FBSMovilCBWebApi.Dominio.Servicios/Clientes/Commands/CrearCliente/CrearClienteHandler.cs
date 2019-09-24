using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands
{
    public class CrearClienteHandler : IRequestHandler<CrearClienteME, CrearClienteMS>
    {

        public CrearClienteHandler()
        {
        }

        public async Task<CrearClienteMS> Handle(CrearClienteME request, CancellationToken cancellationToken)
        {
            return new CrearClienteMS();
        }
    }
}
