using MediatR;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesarPagoME : IRequest<AfectacionMS>
    {
        public double? Comision { get; set; }
        public string NombreCliente { get; set; }
        public string Descripcion { get; set; }

        public int? SecuencialCuentaCliente { get; set; }
        public string CorreoCliente { get; set; }
        public int SecuencialServicio { get; set; }
        public int SecuencialRequerimientoConsulta { get; set; }

        public double Valor { get; set; }
        public string Identificacion { get; set; }

        public IList<CampoPagoResumenME> Campos { get; set; }
    }
}
