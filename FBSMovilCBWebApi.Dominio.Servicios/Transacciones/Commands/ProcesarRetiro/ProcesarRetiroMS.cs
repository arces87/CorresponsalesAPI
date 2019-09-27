using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroMS
    {
        public double Valor { get; set; }
        public string NoCuenta { get; set; }
        public string NoTransaccion { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }
}
