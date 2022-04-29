using Org.OpenAPITools.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro
{
    public class AfectacionAUnCorresponsalRepositorioMS: AfectacionAUnCorresponsalMS
    {
        public bool NotificationError { get; set; } = false;
        public string NotificationErrorMensaje { get; set; } = "";
    }
}
