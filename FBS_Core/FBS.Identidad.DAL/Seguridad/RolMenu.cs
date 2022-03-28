using FBS.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBS.Identidad.DAL.Seguridad
{
    [Table("RolMenu", Schema = "Seguridad")]
    public class RolMenu : IEntidad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Rol Rol { get; set; }
        public Menu Menu { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }

    }
}
