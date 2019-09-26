using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ModeloListaTransaccion
    {
        public string Id { get; set; }

        public DateTime FechaSistema { get; set; }
        public DateTime FechaDispositivo { get; set; }
        public TimeSpan HoraDispositivo { get; set; }
        public string IdAgente { get; set; }
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
