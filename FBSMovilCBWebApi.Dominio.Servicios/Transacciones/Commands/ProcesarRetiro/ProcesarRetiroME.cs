using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroME : IRequest<ProcesarRetiroMS>
    {
        public string Usuario { get; set; }
        public string SecuencialCuenta { get; set; }
        public double Monto { get; set; }
        public string CodigoInsitucion { get; set; }
    }
}
