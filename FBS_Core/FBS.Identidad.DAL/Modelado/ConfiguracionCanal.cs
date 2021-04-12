using System;
using System.Collections.Generic;
using System.Text;

namespace FBS.Identidad.DAL.Modelado
{
    public class ConfiguracionCanal: IConfiguracionCanal
    {
        public string IdCanal { get; set; }
        public IJsonConfiguracion Configuracion { set; get; }
        public JsonNegocio Negocio { set; get; }
    }
}
