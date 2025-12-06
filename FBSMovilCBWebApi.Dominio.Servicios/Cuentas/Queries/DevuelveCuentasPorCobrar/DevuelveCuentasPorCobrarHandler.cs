using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using Corresponsales.Query.Api;
using Corresponsales.Query.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentasPorCobrarHandler : IRequestHandler<DevuelveCuentasPorCobrarME, DevuelveCuentasPorCobrarDeUnClienteResponse>
    {
        private readonly ICuentasPorCobrarApi _cuentasPorCobrarApi;
        private readonly IMediator _mediador;
        

        public DevuelveCuentasPorCobrarHandler(ICuentasPorCobrarApi cuentasPorCobrarApi, IMediator mediador)
        {
            _cuentasPorCobrarApi = cuentasPorCobrarApi;
            _mediador = mediador;            
        }

        public async Task<DevuelveCuentasPorCobrarDeUnClienteResponse> Handle(DevuelveCuentasPorCobrarME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var respuesta = await _cuentasPorCobrarApi.DevuelveCuentasPorCobrarDeUnClienteAsync(request);
            return respuesta;
        }
    }
}
