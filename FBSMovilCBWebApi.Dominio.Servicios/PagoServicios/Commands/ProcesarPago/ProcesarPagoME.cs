using MediatR;
using Org.OpenAPITools.Model;
using System;
using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands
{
    public class ProcesarPagoME : IRequest<PagoFacilitoMSL>
    {
        public double? Comision { get; set; }
        public string NombreCliente { get; set; }
        public string Descripcion { get; set; }
        public int? SecuencialCuentaCliente { get; set; }
        public Guid? IdProducto { get; set; }
        public string Referencia { get; set; }
        public double Valor { get; set; }
        public string ValorTonelaje { get; set; }
        public string Identificacion { get; set; }
        public int? NumeroCuotasPensionesAlimenticiaPersona { get; set; }
        public string CodigoPagarPensionesAlimenticiaEmpresa { get; set; }
        public int? SecuencialResultadoTransaccion { get; set; }
        public bool? ComisionRubro { get; set; }
        public IList<RubroME> Rubros { get; set; }
        
        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
