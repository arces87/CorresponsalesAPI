using FBS.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBS.Identidad.DAL.Seguridad
{
    [Table("Canal", Schema = "Seguridad")]
    public class Canal : IEntidad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string JsonConfiguracion { get; set; }
        public string JsonNegocio { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}