using MediatR;
using Corresponsales.Query.Model;


namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion
{
    public class ListarTiposIdentificacionME: IRequest<DevuelveTiposIdentificacionResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
