using MediatR;
using Corresponsales.Query.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveCuentasPorCobrarME : DevuelveCuentasPorCobrarDeUnClienteRequest, IRequest<DevuelveCuentasPorCobrarDeUnClienteResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
