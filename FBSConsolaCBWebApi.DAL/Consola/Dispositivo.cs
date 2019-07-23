using FBSConsolaCBWebApi.DAL.Nomenclador;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Consola
{
    [Table("Dispositivo", Schema = "Consola")]
    public class Dispositivo
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Imei { get; set; }

        public string Mac { get; set; }

        public string NumeroSerie { get; set; }

        public Catalogo TipoDispositivo { get; set; }

        public bool EstaActivo { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }
    }
}
