using Corresponsales.Command.Model;


namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class ProcesaPagoCuentaMS: ProcesaPagoCuentasPorCobrarResponse
    {

        public bool NotificationError { get; set; } = false;
        public string NotificationErrorMensaje { get; set; } = "";
    }
}
