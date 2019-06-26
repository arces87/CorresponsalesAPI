using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial
{
    [Table("OFICINA", Schema = "ESTRUCTURAEMPRESARIAL")]
    public class Oficina
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

        [Column("CIUDAD")]
        public string Ciudad { get; set; }

        [ForeignKey("EMPRESAID")]
        public Empresa Empresa { get; set; }

        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public IEnumerable<Persona> Personas { get; set; }
    }
}
