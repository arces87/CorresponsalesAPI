using System;
using System.Collections.Generic;
using System.Text;

namespace FBSConsolaCBWebApi.Infraestructura.Utiles
{
    public class FormateadorMensaje: IFormateadorMensaje
    {
        public string FormatearNotificacion(string mensaje,
            string numeroOtp,
            int diasRestantes,
            string nombreUsuario,
            string direccionIp)
        {

            mensaje = mensaje.Replace("#CodigoOTP", numeroOtp)
                             .Replace("#DiasRestantes", diasRestantes.ToString())
                             .Replace("#Comprobante", numeroOtp)
                             .Replace("#usuario", nombreUsuario)
                             .Replace("#Ip", direccionIp)
                             .Replace("#Hora", DateTime.Now.ToString())
                             .Replace("[:CodigoOTP:]", numeroOtp)
                             .Replace("[:DiasRestantes:]", diasRestantes.ToString())
                             .Replace("[:Comprobante:]", numeroOtp)
                             .Replace("[:usuario:]", nombreUsuario)
                             .Replace("[:ipMaquina:]", direccionIp)
                             .Replace("[:fecha:]", DateTime.Now.ToString("dd/MM/yyyy"));
            return mensaje;
        }

        //public static string FormatearNotificacionOperacionExitosa(string mensaje, string cuentaOrigen,
        //    string cuentaDestino,
        //    double monto,
        //    string concepto,
        //    string fecha)
        //{

        //    mensaje = mensaje
        //                     .Replace("[:cuentaOrigen:]", cuentaOrigen)
        //                     .Replace("[:cuentaDestino:]", cuentaDestino)
        //                     .Replace("[:monto:]", monto.ToString("C", CultureInfo.CurrentCulture))
        //                     .Replace("[:concepto:]", concepto)
        //                     .Replace("[:fecha:]", fecha);
        //    return mensaje;
        //}
    }
}
