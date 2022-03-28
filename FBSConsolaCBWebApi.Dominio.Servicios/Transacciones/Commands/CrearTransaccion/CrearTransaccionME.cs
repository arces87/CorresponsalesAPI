using MediatR;
using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class CrearTransaccionME : IRequest<string>
    {
        public DateTime FechaSistema { get; set; }
        public DateTime FechaDispositivo { get; set; }
        public TimeSpan HoraDispositivo { get; set; }
        public string IdAgente { get; set; }
        public string Criptografia { get; set; }
        public string Tipo { get; set; }
        public string JsonDatos { get; set; }
        public string IdEstado { get; set; }
        public double Valor { get; set; }
        public double SaldoDisponible { get; set; }
        public string Comisiones { get; set; }
        public string CanalId { get; set; }
        public bool ReposicionRealizada { get; set; }
    }
}
