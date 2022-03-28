using Newtonsoft.Json;
using System.ComponentModel;

namespace FBS.Identidad.Dominio.Servicios.Canales.Queries
{
    public class Limite
    {
        [JsonProperty("numeroMaximoDiarioDeTransacciones")]
        public int? NumeroMaximoDiarioDeTransacciones { get; set; }
        [JsonProperty("montoMaximoDiarioDeTransacciones")]
        public double? MontoMaximoDiarioDeTransacciones { get; set; }
        [JsonProperty("saldoMaximoCuentaAsociada")]
        public double? SaldoMaximoCuentaAsociada { get; set; }
        [JsonProperty("saldoMaximoAgente")]
        public double? SaldoMaximoAgente { get; set; }
        
        [DefaultValue(0)]
        [JsonProperty("existenciaCaja", DefaultValueHandling = DefaultValueHandling.Populate)]
        public double? ExistenciaCaja { get; set; }
    }
}