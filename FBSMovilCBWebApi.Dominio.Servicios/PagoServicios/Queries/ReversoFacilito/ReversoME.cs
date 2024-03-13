using Corresponsales.Command.Model;
using MediatR;


namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ReversoME : ReversoFacilitoRequest, IRequest<ReversoFacilitoResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
