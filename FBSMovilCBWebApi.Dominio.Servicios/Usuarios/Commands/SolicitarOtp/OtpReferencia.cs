using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class OtpReferencia
    {

        [JsonProperty("referencia")]
        public string Referencia { get; set; }
        

        [JsonProperty("intentosFallidos")]
        public int IntentosFallidos { get; set; } = 0;
    }
}
