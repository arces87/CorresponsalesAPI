using MediatR;
using Org.OpenAPITools.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries.ListarPrestamos
{
    public class ListarPrestamosME: PorIdentificacionClienteActivaME, IRequest<InformacionPrestamosMSL>
    {
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
