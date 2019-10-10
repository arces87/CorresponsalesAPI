using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ModeloListarHojaColecta
    {
        public string Tipo { get; set; }
        public double Valor { get; set; }
        public DateTime FechaSistema { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string NumeroCuenta { get; set; }
    }
}