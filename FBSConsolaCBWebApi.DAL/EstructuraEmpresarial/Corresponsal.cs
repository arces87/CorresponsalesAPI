using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.EstructuraEmpresarial
{
    [Table("Corresponsal", Schema = "EstructuraEmpresarial")]
    public class Corresponsal
    {
        [Key]
        [ForeignKey(nameof(Persona))]
        public int Id { get; set; }

        public Persona Persona { get; set; }

        public float Latitud { get; set; }

        public float Longitud { get; set; }

        public Supervisor Supervisor { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }


    }
}
