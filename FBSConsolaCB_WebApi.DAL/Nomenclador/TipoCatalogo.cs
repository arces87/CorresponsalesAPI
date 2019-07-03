using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.Nomenclador
{
    [Table("TipoCatalogo", Schema = "Nomenclador")]
    public class TipoCatalogo
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }

        public IEnumerable<Catalogo> Catalogos { get; set; }
    }
}
