using Corresponsales.Query.Api;
using Corresponsales.Query.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Generales.Queries
{
    public class DevuelveNombreEmpresaHandler : IRequestHandler<DevuelveNombreEmpresaME, DevuelveNombreEmpresaResponse>
    {
        private readonly IGeneralesApi _generalesApi;

        public DevuelveNombreEmpresaHandler(IGeneralesApi generalesApi)
        {
            _generalesApi = generalesApi;
        }

        public async Task<DevuelveNombreEmpresaResponse> Handle(DevuelveNombreEmpresaME request, CancellationToken cancellationToken)
        {
            var requestModel = new DevuelveNombreEmpresaRequest(request.Secuencial);
            var respuesta = await _generalesApi.DevuelveNombreEmpresaAsync(requestModel, cancellationToken: cancellationToken);

            return respuesta;
        }
    }
}

