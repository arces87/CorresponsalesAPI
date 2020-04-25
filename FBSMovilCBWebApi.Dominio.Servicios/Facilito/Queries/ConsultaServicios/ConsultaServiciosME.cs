using MediatR;
using ServiciosFinancial.Models;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ConsultaServiciosME : IRequest<ConsultaValorAPagarMS>
    {
        public Guid? IdProducto { get; set; }
        public string Referencia { get; set; }
        public string Identificacion { get; set; }
        public string ValorTonelaje { get; set; }
        public int? NumeroCuotasPensionesAlimenticiaPersona { get; set; }
        public string CodigoPagarPensionesAlimenticiaEmpresa { get; set; }
    }
}
