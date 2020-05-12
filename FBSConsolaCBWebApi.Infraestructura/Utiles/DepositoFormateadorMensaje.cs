using FBSConsolaCBWebApi.DAL.Corresponsales;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSConsolaCBWebApi.Infraestructura.Utiles
{
    public class DepositoFormateadorMensaje: IFormateadorMensaje<Transaccion>
    {
        public string FormatearEmail(string mensaje, Transaccion operacion)
        {
            throw new NotImplementedException();
        }

        public string FormatearSMS(string mensaje, Transaccion operacion)
        {
            throw new NotImplementedException();
        }
     
    }
}
