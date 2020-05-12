using System;
using MediatR;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FBSServiciosSMSTulcan.EnviarSMS
{
    public class EnviarSmsLoteME: IRequest<EnviarSmsLoteMS>
    { 

        [JsonProperty(PropertyName = "recipients")]
        public List<string> Destinatario { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Mensaje { get; set; }
    }
}
