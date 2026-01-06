using Corresponsales.Query.Model;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class DevuelveCategoriasServiciosME : IRequest<DevuelveCategoriaResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
        public int SecuencialEmpresa { get; set; }
    }
}