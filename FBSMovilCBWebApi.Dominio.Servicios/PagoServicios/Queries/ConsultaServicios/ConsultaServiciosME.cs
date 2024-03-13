using MediatR;
using System;
using Corresponsales.Query.Model;
namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ConsultaServiciosME : ConsultaValorAPagarRequest, IRequest<ConsultaValorAPagarResponse>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
