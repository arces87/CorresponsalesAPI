using MediatR;
using Org.OpenAPITools.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarAbonoPrestamoME : IRequest<EfectivizacionPrestamoMS>
    {
        public int SecuencialCuentaCliente { get; set; }
        public string NumeroPrestamo { get; set; }    
        public double Valor { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string Concepto { get; set; }
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
