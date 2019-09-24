using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveTipoCuentaME : IRequest<DevuelveTipoCuentaMS>
    {
        public string Secuencial { get; set; }
    }
}
