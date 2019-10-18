using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class CrearCuentaME : IRequest<CreaCuentaMSL>
    {
        public string CodigoTipoCuenta { get; set; }
        public int SecuencialCliente { get; set; }
    }
}
