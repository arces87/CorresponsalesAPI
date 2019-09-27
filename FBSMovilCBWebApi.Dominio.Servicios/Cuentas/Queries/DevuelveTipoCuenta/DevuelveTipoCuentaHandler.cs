using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveTipoCuentaHandler : IRequestHandler<DevuelveTipoCuentaME, DevuelveTipoCuentaMS>
    {

        public DevuelveTipoCuentaHandler()
        {
        }

        public async Task<DevuelveTipoCuentaMS> Handle(DevuelveTipoCuentaME request, CancellationToken cancellationToken)
        {
            return new DevuelveTipoCuentaMS()
            {
                TiposCuentas = new List<ModeloTipoCuenta>() {
                new ModeloTipoCuenta() { Secuencial="12345",
                Codigo="DE",
                Nombre = "Débito"
                },
                new ModeloTipoCuenta() { Secuencial="12346",
                Codigo="CR",
                Nombre = "Crédito"
                }}
            };
        }
    }
}
