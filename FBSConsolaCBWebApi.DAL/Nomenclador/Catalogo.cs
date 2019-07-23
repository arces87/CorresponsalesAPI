using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Nomenclador
{
    [Table("Catalogo", Schema = "Nomenclador")]
    public class Catalogo
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool EstaActivo { get; set; }

        public TipoCatalogo TipoCatalogo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
