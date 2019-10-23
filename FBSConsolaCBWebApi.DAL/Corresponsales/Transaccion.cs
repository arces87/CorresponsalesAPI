using FBS.DAL.Nomenclador;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Corresponsales
{
    [Table("Transaccion", Schema = "Corresponsales")]
    public class Transaccion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Agente Agente { get; set; }
        public Catalogo Estado { get; set; }
        public string CanalId { get; set; }
        public DateTime FechaSistema { get; set; }
        public DateTime FechaDispositivo { get; set; }
        public TimeSpan HoraDispositivo { get; set; }
        public string Criptografia { get; set; }
        public string Tipo { get; set; }
        public string JsonDatos { get; set; }
        public double Valor { get; set; }
        public double SaldoDisponible { get; set; }
        public double SaldoCuenta { get; set; }
        public string Comisiones { get; set; }
        public bool ReposicionRealizada { get; set; }
        public string Descripcion { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string NumeroCuenta { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
