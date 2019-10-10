using MediatR;
using ServiciosFacilito.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Facilito.Commands
{
    public class ProcesarPagoME : IRequest<PagoResponse>
    {
        public int? CodigoTransaccion { get; set; }
        public string Nombres { get; set; }
        public int? Operador { get; set; }
        public string Producto { get; set; }
        public string Referencia { get; set; }
        public int? ServicioBancario { get; set; }
        public double? Comision { get; set; }
        public string XmlData { get; set; }
        public double Valor { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string Descripcion { get; set; }
    }
}
