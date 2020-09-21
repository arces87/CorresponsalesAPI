using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTipoTransaccionME : IRequest<ListarTipoTransaccionMS>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}