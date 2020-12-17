using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro
{
    public class AfectacionAUnCorresponsalRepositorioMS: AfectacionAUnCorresponsalMS
    {
        public bool NotificationError { get; set; } = false;
        public string NotificationErrorMensaje { get; set; } = "";
    }
}
