using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Canales
{
    [Table("Log", Schema = "Canales")]
    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Criptografia { get; set; }
        public string JsonDispositivo { get; set; }
        public string JsonLog { get; set; }
        public Catalogo TipoAccion { get; set; }
        public Catalogo Estado { get; set; }
        public string RelacionadoId { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
