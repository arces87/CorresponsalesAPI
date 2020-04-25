using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarAbonoPrestamoME : IRequest<EfectivizacionPrestamoMS>
    {
        public int SecuencialCuenta { get; set; }
        public string NumeroPrestamo { get; set; }
        public double Valor { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string Concepto { get; set; }
    }
}
