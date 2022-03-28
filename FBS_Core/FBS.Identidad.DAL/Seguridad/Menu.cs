using FBS.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBS.Identidad.DAL.Seguridad
{
    [Table("Menu", Schema = "Seguridad")]
    public class Menu : IEntidad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("MenuPadreId")]
        public Guid? MenuId { get; set; }

        public string Nombre { get; set; }

        public int Orden { get; set; }

        public string Icono { get; set; }

        public string Ruta { get; set; }

        public IEnumerable<Menu> Menus { get; set; }

        public virtual IEnumerable<RolMenu> RoleMenu { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}