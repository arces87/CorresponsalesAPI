using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.Consola
{
    [Table("LimiteTransaccional", Schema = "Consola")]
    public class LimiteTransaccional
    {
        [Key]
        public int Id { get; set; }

        public double Monto { get; set; }

        public int Dias { get; set; }

        public Corresponsal Corresponsal { get; set; }

        public Catalogo Operacion { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
