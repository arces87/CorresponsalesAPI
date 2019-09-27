using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaHandler : IRequestHandler<DevuelveCuentaME, DevuelveCuentaMS>
    {

        public DevuelveCuentaHandler()
        {
        }

        public async Task<DevuelveCuentaMS> Handle(DevuelveCuentaME request, CancellationToken cancellationToken)
        {
            return new DevuelveCuentaMS()
            {
                TiposCuentas = new List<ModeloCuenta>() {
                new ModeloCuenta() {
                    Secuencial ="12345",
                    NoCuenta="45345324",
                    Disponible = 200,
                    TipoCuenta ="DE"

                },
                new ModeloCuenta() {
                    Secuencial ="12346",
                    NoCuenta="45365324",
                    Disponible = 300,
                    TipoCuenta ="CR"
                }}
            };
        }
    }
}
