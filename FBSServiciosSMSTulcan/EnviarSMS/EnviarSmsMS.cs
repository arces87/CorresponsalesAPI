using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSServiciosSMSTulcan.EnviarSMS
{
    public class EnviarSmsMS
    {
        [JsonProperty(PropertyName = "status")]
        public string Estado { get; set; }
        
        [JsonProperty(PropertyName = "errorMessage")]
        public string MensajeError { get; set; }   
        
        [JsonProperty(PropertyName = "confirmationNumber")]
        public string NumeroConfirmacion { get; set; }
    }
}
