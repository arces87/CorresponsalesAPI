using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Consola
{
    [Table("Alerta", Schema = "Consola")]
    public class Alerta
    {
        [Key]
        public int Id { get; set; }

        public int IdConversacion { get; set; }

        public Persona Destinatario { get; set; }

        public Persona Remitente { get; set; }

        public string Asunto { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public int Estado { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
