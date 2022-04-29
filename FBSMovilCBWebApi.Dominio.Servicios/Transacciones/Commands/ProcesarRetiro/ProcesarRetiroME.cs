using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro;
using MediatR;


namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroME : IRequest<AfectacionAUnCorresponsalRepositorioMS>
    {
        public int SecuencialCuenta { get; set; }
        public string NumeroCuentaCliente { get; set; }
        public string TipoCuentaCliente { get; set; }
        public double Valor { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public int TipoIdentificacionCliente { get; set; }
        public string Descripcion { get; set; }

        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
