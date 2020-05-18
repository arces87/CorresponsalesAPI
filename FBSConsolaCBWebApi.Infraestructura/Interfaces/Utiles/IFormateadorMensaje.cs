using System;
using System.Collections.Generic;
using System.Text;

namespace FBSConsolaCBWebApi.Infraestructura.Utiles
{
    public interface IFormateadorMensaje<T>
    {
        string FormatearSMS(string mensaje, T operacion);
        string FormatearEmail(string mensaje, T operacion);
    }
}
