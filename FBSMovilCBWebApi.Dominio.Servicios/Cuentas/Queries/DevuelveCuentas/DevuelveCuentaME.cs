using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaME : IRequest<ConsolidadoCuentasMSL>
    {
        public string CodigoInsitucion { get; set; }
        public string SecuencialCliente { get; set; }
    }
}
