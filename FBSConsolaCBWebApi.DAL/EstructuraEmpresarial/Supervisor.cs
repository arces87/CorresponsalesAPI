using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.EstructuraEmpresarial
{
    [Table("Supervisor", Schema = "EstructuraEmpresarial")]
    public class Supervisor
    {
        [Key]
        [ForeignKey(nameof(Persona))]
        public int Id { get; set; }

        public Persona Persona { get; set; }

        public IEnumerable<Corresponsal> Corresponsales { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }


    }
}
