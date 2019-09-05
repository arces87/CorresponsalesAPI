using FBS.DAL.Nomenclador;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Canales
{
    [Table("Dispositivo", Schema = "Canales")]
    public class Dispositivo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public bool TieneImpresora { get; set; }
        public string DireccionImpresora { get; set; }
        public string Observaciones { get; set; }
        public string Ubicacion { get; set; }
        public string Imei { get; set; }
        public Catalogo Marca { get; set; }
        public Catalogo SistemaOperativo { get; set; }

        public bool EstaActivo { get; set; }
        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
