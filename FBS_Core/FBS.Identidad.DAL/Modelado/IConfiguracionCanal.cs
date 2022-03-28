using System;
using System.Collections.Generic;
using System.Text;

namespace FBS.Identidad.DAL.Modelado
{
    public interface IConfiguracionCanal
    {
        string IdCanal { get; set; }
        IJsonConfiguracion Configuracion { set; get; }
        JsonNegocio Negocio { set; get; }
    }
}
