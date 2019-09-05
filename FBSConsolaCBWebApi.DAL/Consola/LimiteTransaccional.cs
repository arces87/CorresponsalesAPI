using FBS.DAL.Nomenclador;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Consola
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
