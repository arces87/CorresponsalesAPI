using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class CrearCuentaHandler : IRequestHandler<CrearCuentaME, CrearCuentaMS>
    {

        public CrearCuentaHandler()
        {
        }

        public async Task<CrearCuentaMS> Handle(CrearCuentaME request, CancellationToken cancellationToken)
        {
            return new CrearCuentaMS();
        }
    }
}
