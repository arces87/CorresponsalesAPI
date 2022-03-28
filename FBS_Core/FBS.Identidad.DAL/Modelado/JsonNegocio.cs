using Newtonsoft.Json;

namespace FBS.Identidad.DAL.Modelado
{
    public class JsonNegocio
    {
        [JsonProperty("limites")]
        public Limite Limites { get; set; }

        [JsonProperty("deposito")]
        public Operacion Deposito { get; set; }
        [JsonProperty("retiro")]
        public Operacion Retiro { get; set; }
        [JsonProperty("cobroServicios")]
        public Operacion CobroServicios { get; set; }

        [JsonProperty("abonoPrestamos")]
        public Operacion AbonoPrestamos { get; set; }

        [JsonProperty("verificarGeolocalizacion")]
        public bool VerificarGeolocalizacion { get; set; }

        [JsonProperty("tiempoBloqueo")]
        public int TiempoBloqueo { get; set; }

        [JsonProperty("numeroMaximoIntentosFallidos")]
        public int NumeroMaximoIntentosFallidos { get; set; }

        [JsonProperty("diasValidosContrasenna")]
        public int DiasValidosContrasenna { get; set; }

        [JsonProperty("numeroMaximoIntentosFallidosOtp")]
        public int NumeroMaximoIntentosFallidosOtp { get; set; } = 3;

        [JsonProperty("tiempoVidaTokenHoras")]
        public int TiempoVidaToken { get; set; } = 60;
    }
}
