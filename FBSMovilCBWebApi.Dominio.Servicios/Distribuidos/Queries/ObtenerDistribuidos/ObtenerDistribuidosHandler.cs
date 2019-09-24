using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Distribuidos.Queries
{
    public class ObtenerDistribuidosHandler : IRequestHandler<ObtenerDistribuidosME, ObtenerDistribuidosMS>
    {

        public ObtenerDistribuidosHandler()
        {
        }

        public async Task<ObtenerDistribuidosMS> Handle(ObtenerDistribuidosME request, CancellationToken cancellationToken)
        {
            return new ObtenerDistribuidosMS();
        }
    }
}
