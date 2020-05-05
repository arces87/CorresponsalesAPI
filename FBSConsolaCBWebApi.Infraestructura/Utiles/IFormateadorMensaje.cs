using System;
using System.Collections.Generic;
using System.Text;

namespace FBSConsolaCBWebApi.Infraestructura.Utiles
{
    public interface IFormateadorMensaje
    {
        string FormatearNotificacion(string mensaje, string numeroOtp, int diasRestantes, string nombreUsuario, string direccionIp);
    }
}
