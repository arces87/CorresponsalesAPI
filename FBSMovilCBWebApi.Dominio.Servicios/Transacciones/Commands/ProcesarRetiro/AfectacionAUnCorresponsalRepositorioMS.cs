using Corresponsales.Command.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro
{
    public class AfectacionAUnCorresponsalRepositorioMS: AfectacionCorresponsalResponse
    {
        public bool NotificationError { get; set; } = false;
        public string NotificationErrorMensaje { get; set; } = "";
    }
}
