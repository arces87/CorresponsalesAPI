using FBS.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBS.Identidad.DAL.Seguridad
{
    [Table("CanalUsuario", Schema = "Seguridad")]
    public class CanalUsuario : IEntidad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Canal Canal { get; set; }
        public Usuario Usuario { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }

    }
}
