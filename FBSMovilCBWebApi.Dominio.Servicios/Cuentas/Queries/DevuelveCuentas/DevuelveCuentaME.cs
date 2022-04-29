using MediatR;
using Org.OpenAPITools.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentaME : PorClienteDeUnaEmpresaME, IRequest<ConsolidadoCuentasMSL>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
