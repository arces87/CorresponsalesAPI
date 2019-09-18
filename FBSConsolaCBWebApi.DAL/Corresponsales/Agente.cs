using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
using FBSConsolaCBWebApi.DAL.Canales;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Corresponsales
{
    [Table("Agente", Schema = "Corresponsales")]
    public class Agente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string NombreAgente { get; set; }

        public string JsonAgente { get; set; }
        public string Identificacion { get; set; }
        public string Ubicacion { get; set; }
        public Catalogo Estado { get; set; }

        public Usuario Usuario { get; set; }

        public Usuario Supervisor { get; set; }

        public Dispositivo Dispositivo { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }


    }
}
