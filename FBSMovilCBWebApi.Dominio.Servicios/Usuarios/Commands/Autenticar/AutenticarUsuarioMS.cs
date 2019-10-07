namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioMS
    {
        public string Token { get; set; }
        public bool ValidarOtpAgente { get; set; }
        public bool ValidarOtpCliente { get; set; }
        public string Identificacion { get; set; }
        public ComisionesMS Comisiones { get; set; }
    }
}
