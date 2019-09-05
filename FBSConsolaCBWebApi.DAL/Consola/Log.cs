using FBS.DAL.Nomenclador;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Consola
{
    [Table("Log", Schema = "Consola")]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string Transaccion { get; set; }

        public DateTime Fecha { get; set; }

        public string Canal { get; set; }

        public string Json { get; set; }

        public Catalogo Operacion { get; set; }

        public Corresponsal Corresponsal { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
