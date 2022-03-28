using Newtonsoft.Json;

namespace FBS.Identidad.DAL.Modelado
{
    public class ComisionOperacion
    {
        [JsonProperty("agente")]
        public double? Agente { get; set; }
        [JsonProperty("administracionCanal")]
        public double? AdministracionCanal { get; set; }
        [JsonProperty("cooperativa")]
        public double? Cooperativa { get; set; }
        [JsonProperty("gravaIva")]
        public bool? GravaIva { get; set; }
    }
}