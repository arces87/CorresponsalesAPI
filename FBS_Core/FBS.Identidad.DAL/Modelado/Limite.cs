using Newtonsoft.Json;
using System.ComponentModel;

namespace FBS.Identidad.DAL.Modelado
{
    public class Limite
    {
        [JsonProperty("numeroMaximoDiarioDeTransacciones")]
        public int? NumeroMaximoDiarioDeTransacciones { get; set; }
        [JsonProperty("montoMaximoDiarioDeTransacciones")]
        public double? MontoMaximoDiarioDeTransacciones { get; set; }
        [DefaultValue(0)]
        [JsonProperty("saldoMaximoCuentaAsociada", DefaultValueHandling = DefaultValueHandling.Populate)]
        public double? SaldoMaximoCuentaAsociada { get; set; }
        [JsonProperty("saldoMaximoAgente")]
        public double? SaldoMaximoAgente { get; set; }
        
        [DefaultValue(0)]
        [JsonProperty("existenciaCaja", DefaultValueHandling = DefaultValueHandling.Populate)]
        public double? ExistenciaCaja { get; set; }
    }
}