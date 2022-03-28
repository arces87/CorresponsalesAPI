using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Canales
{
    [Table("Geolocalizacion", Schema = "Canales")]
    public class Geolocalizacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
