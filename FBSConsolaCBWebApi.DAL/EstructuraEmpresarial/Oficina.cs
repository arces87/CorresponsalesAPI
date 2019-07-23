using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.EstructuraEmpresarial
{
    [Table("Oficina", Schema = "EstructuraEmpresarial")]
    public class Oficina
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool EstaActivo { get; set; }

        public string Ciudad { get; set; }

        public Empresa Empresa { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }

        public IEnumerable<Persona> Personas { get; set; }
    }
}
