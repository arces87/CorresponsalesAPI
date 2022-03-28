using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBS.DAL.Nomenclador
{
    [Table("Catalogo", Schema = "Nomenclador")]
    public class Catalogo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public TipoCatalogo TipoCatalogo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
