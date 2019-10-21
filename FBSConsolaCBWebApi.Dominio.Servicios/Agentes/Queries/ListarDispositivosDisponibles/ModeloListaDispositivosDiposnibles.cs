namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ModeloListaDispositivosDiposnibles
    {
        public string Id { get; set; }
        public string MacAddress { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public bool TieneImpresora { get; set; }
        public string DireccionImpresora { get; set; }
        public string Ubicacion { get; set; }
        public string Imei { get; set; }
    }
}
