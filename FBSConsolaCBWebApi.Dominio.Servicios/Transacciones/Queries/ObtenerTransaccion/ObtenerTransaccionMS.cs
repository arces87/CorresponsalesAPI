using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerTransaccionMS
    {
        public string Id { get; set; }

        public DateTime FechaSistema { get; set; }
        public DateTime FechaDispositivo { get; set; }
        public string HoraDispositivo { get; set; }
        public string IdAgente { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string SecuencialCuenta { get; set; }
        public string Tipo { get; set; }
        public string NombreAgente { get; set; }
        public string IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public double Valor { get; set; }
        public double SaldoDisponible { get; set; }
        public string Comisiones { get; set; }
        public string Descripcion { get; set; }
        public bool ReposicionRealizada { get; set; }
    }
}
