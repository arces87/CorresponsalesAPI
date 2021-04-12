using System.Collections.Generic;

namespace FBS.Identidad.DAL.Modelado
{
    public class JsonConfiguracion : IJsonConfiguracion
    {
        public string IdCanal { get; set; }
        public IEnumerable<Parametrizacion> Parametrizaciones { get; set; }
        public int TiempoVidaOtp { get; set; }
    }
}
