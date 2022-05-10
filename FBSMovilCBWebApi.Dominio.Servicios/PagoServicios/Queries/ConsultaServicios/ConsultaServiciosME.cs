using MediatR;
using Org.OpenAPITools.Model;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ConsultaServiciosME : ConsultaValorAPagarME, IRequest<ConsultaValorAPagarMS>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
