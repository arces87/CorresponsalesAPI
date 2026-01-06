using Corresponsales.Query.Model;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class DevuelveServiciosPorCategoriaME : IRequest<DevuelveServiciosResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
        public string NombreCategoria { get; set; }
    }
}