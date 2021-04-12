using MediatR;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion
{
    public class ListarTiposIdentificacionME: IRequest<TiposIdentificacionMSL>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
