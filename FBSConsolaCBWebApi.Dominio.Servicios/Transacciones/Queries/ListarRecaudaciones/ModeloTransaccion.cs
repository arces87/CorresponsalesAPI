using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ModeloTransaccion
    {
        public string NombreCliente { get; set; }
        public DateTime FechaSistema { get; set; }
        public DateTime FechaDispositivo { get; set; }
        public string HoraDispositivo { get; set; }
        public string Comisiones { get; set; }
        public double Valor { get; set; }
    }
}