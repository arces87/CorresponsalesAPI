using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerDispositivoMS
    {
        public string Id { get; set; }
        public string MacAddress { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public bool TieneImpresora { get; set; }
        public string DireccionImpresora { get; set; }
        public string Observaciones { get; set; }
        public string Ubicacion { get; set; }
        public string Imei { get; set; }
        public string IdMarca { get; set; }
        public string NombreMarca { get; set; }
        public string IdSistemaOperativo { get; set; }
        public string NombreSistemaOperativo { get; set; }
        public IEnumerable<ObtenerDispositivoImagen> Imagenes { get; set; }
    }
}
