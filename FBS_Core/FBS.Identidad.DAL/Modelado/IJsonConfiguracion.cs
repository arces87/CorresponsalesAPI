using System.Collections.Generic;

namespace FBS.Identidad.DAL.Modelado
{
    public interface IJsonConfiguracion
    {
        string IdCanal { get; set; }
        IEnumerable<Parametrizacion> Parametrizaciones { get; set; }
        int TiempoVidaOtp { get; set; }
    }
}
