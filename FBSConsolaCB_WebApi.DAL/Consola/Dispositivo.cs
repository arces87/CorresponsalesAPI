using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.Consola
{
    [Table("DISPOSITIVO", Schema = "CONSOLA")]
    public class Dispositivo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("IMEI")]
        public string Imei { get; set; }

        [Column("MAC")]
        public string Mac { get; set; }

        [Column("NUMEROSERIE")]
        public string NumeroSerie { get; set; }

        [ForeignKey("TIPODISPOSITIVOID")]
        public Catalogo TipoDispositivo { get; set; }

        [ForeignKey("CORRESPONSALID")]
        public Persona Persona { get; set; }

        [Column("ESTAACTIVO")]
        public bool EstaActivo { get; set; }

        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
