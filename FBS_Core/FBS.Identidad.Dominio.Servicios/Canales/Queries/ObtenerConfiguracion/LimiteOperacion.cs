using Newtonsoft.Json;

namespace FBS.Identidad.Dominio.Servicios.Canales.Queries
{
    public class LimiteOperacion
    {
        [JsonProperty("numeroMaximoDiarioDeTransacciones")]
        public int? NumeroMaximoDiarioDeTransacciones { get; set; }
        [JsonProperty("montoMaximoDiarioDeTransacciones")]
        public double? MontoMaximoDiarioDeTransacciones { get; set; }
        [JsonProperty("montoMinimoPorTransaccion")]
        public double? MontoMinimoPorTransaccion { get; set; }
        [JsonProperty("montoMaximoPorTransaccion")]
        public double? MontoMaximoPorTransaccion { get; set; }
    }
}