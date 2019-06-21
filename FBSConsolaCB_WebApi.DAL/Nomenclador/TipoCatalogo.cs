using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.Nomenclador
{
    [Table("TIPOCATALOGO", Schema = "NOMENCLADOR")]
    public class TipoCatalogo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("ESTAACTIVO")]
        public bool EstaActivo { get; set; }

        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public IEnumerable<Catalogo> Catalogos { get; set; }
    }
}
