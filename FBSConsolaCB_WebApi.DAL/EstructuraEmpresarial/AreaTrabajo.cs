using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial
{
    [Table("AREATRABAJO", Schema = "ESTRUCTURAEMPRESARIAL")]
    public class AreaTrabajo
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

        public IEnumerable<Corresponsal> Corresponsales { get; set; }
    }
}
