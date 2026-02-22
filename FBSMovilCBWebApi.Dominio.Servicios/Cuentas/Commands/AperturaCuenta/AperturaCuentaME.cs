using Corresponsales.Command.Model;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands.AperturaCuenta
{
    public class AperturaCuentaME : IRequest<AperturaCuentaResponse>
    {
        public int SecuencialCuentaSocio { get; set; }
        public int SecuencialCuentaCorresponsal { get; set; }
        public int SecuencialCliente { get; set; }
        public decimal ValorApertura { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }

        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
