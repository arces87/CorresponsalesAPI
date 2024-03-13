using Corresponsales.Query.Model;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteME : DevuelveDatosPersonaIdentificacionRequest, IRequest<DevuelveDatosPersonaIdentificacionResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
