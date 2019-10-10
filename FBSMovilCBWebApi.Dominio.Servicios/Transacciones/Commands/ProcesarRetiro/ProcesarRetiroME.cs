using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroME : IRequest<RespuestaProcesoRetiroMS>
    {
        public string NumeroCuenta { get; set; }
        public double Valor { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string Descripcion { get; set; }
    }
}
