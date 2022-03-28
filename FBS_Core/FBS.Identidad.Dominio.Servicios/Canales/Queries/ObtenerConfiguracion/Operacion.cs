using Newtonsoft.Json;

namespace FBS.Identidad.Dominio.Servicios.Canales.Queries
{
    public class Operacion
    {
        [JsonProperty("activo")]
        public bool? Activo { get; set; }

        [JsonProperty("limites")]
        public LimiteOperacion Limites { get; set; }

        [JsonProperty("comisiones")]
        public ComisionOperacion Comisiones { get; set; }

        [JsonProperty("validarOtpAgente")]
        public bool ValidarOtpAgente { get; set; }
        [JsonProperty("validarOtpCliente")]
        public bool ValidarOtpCliente { get; set; }
        [JsonProperty("notificarCorreoElectronico")]
        public bool NotificarCorreoElectronico { get; set; }
        [JsonProperty("notificarSMS")]
        public bool NotificarSMS { get; set; }
        [JsonProperty("plantillaCorreoElectronico")]
        public string PlantillaCorreoElectronico { get; set; }
        [JsonProperty("plantillaSMS")]
        public string PlantillaSMS { get; set; }
    }
}