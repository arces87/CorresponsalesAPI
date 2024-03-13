using Corresponsales.Command.Model;


namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarDeposito
{
    public class AfectacionAUnCorresponsalDepositoMS: AfectacionCorresponsalResponse
    {

        public bool NotificationError { get; set; } = false;
        public string NotificationErrorMensaje { get; set; } = "";
    }
}
