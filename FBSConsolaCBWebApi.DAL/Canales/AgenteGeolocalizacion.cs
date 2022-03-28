using FBSConsolaCBWebApi.DAL.Corresponsales;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Canales
{
    [Table("AgenteGeolocalizacion", Schema = "Canales")]
    public class AgenteGeolocalizacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Agente Agente { get; set; }
        public Geolocalizacion Geolocalizacion { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
