using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Corresponsales.Command.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class CrearCuentaME : IRequest<CreaCuentaResponse>
    {
        public string CodigoTipoCuenta { get; set; }
        public int SecuencialCliente { get; set; }

        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
