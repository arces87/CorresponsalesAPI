using Org.OpenAPITools.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarDeposito
{
    public class AfectacionAUnCorresponsalDepositoMS: AfectacionAUnCorresponsalMS
    {

        public bool NotificationError { get; set; } = false;
        public string NotificationErrorMensaje { get; set; } = "";
    }
}
