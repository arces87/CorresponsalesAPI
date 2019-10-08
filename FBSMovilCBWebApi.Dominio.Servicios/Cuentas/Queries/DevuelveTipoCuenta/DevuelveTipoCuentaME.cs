using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveTipoCuentaME : IRequest<TiposCuentaClienteMSL>
    {
        public int SecuencialCliente { get; set; }
        public int SecuencialEmpresa { get; set; }
        public string CodigoProductoVista { get; set; }
    }
}
