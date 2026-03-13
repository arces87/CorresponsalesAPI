using Corresponsales.Query.Model;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.InformacionCuotas
{
    public class InformacionCuotasME : DevuelveInformacionCuotasValorACobrarRequest, IRequest<NumeroCuotasValorAdelantoListaResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
