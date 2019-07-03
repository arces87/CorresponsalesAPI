using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.Consola
{
    [Table("LimiteExistencia", Schema = "Consola")]
    public class LimiteExistencia
    {
        [Key]
        public int Id { get; set; }

        public double Limite { get; set; }

        public Corresponsal Corresponsal { get; set; }
        
        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
