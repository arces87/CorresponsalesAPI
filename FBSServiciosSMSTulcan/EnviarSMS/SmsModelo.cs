using Newtonsoft.Json;
using System;

namespace FBSServiciosSMSTulcan.EnviarSMS
{
    public class SmsModelo
    {

        [JsonProperty(PropertyName = "recipient")]
        public string Destinatario { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Mensaje { get; set; }
    }
}
