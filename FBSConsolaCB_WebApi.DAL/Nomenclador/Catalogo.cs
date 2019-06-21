using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.Nomenclador
{
    [Table("CATALOGO", Schema = "NOMENCLADOR")]
    public class Catalogo
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

        [ForeignKey("TIPOCATALOGOID")]
        public TipoCatalogo TipoCatalogo { get; set; }

        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
