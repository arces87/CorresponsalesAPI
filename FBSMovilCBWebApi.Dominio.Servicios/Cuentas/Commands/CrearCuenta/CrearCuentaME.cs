using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class CrearCuentaME : IRequest<CreaCuentaMS>
    {
        public string CodigoTipoCuenta { get; set; }
        public int SecuencialCliente { get; set; }
    }
}
