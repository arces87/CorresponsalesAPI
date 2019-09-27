using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaME : IRequest<DevuelveCuentaMS>
    {
        public string CodigoInsitucion { get; set; }
        public string Secuencial { get; set; }
    }
}
