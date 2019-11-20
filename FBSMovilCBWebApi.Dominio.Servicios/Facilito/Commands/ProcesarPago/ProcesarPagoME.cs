using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Facilito.Commands
{
    public class ProcesarPagoME : IRequest<PagoFacilitoMS>
    {
        public int SecuencialCuenta { get; set; }
        public double? Comision { get; set; }
        public double Valor { get; set; }
        public string JsonFacilito { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string Descripcion { get; set; }
    }
}
