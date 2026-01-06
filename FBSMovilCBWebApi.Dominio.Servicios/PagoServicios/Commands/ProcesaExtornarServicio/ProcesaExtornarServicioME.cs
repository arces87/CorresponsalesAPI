using Corresponsales.Command.Model;
using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesaExtornarServicioME : IRequest<ProcesaExtornarServicioResponse>
    {
        public ExtornarServicioRequest Request { get; set; }
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}