using MediatR;
using ServiciosFacilito.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ConsultaServiciosME : IRequest<ConsultaResponse>
    {
        public int? CodigoTransaccion { get; set; }
        public int? Operador { get; set; }
        public string Producto { get; set; }
        public string CodigoAuxiliar { get; set; }
        public string Referencia { get; set; }
        public int? ServicioBancario { get; set; }
        public string XmlAdd { get; set; }
    }
}
