using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Canales
{
    [Table("ImagenGeolocalizacion", Schema = "Canales")]
    public class ImagenGeolocalizacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string DireccionImagen { get; set; }

        public Geolocalizacion Geolocalizacion { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
