using Corresponsales.Query.Model;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class DevuelveDetalleDelServicioME : IRequest<DevuelveServicioDetalleResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
        public string IdServicio { get; set; }
        public string Valor { get; set; }
    }
}