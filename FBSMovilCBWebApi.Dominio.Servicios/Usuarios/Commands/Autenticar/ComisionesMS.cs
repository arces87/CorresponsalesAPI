namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class ComisionesMS
    {
        public ComisionOperacionMS Deposito { get; set; }
        public ComisionOperacionMS Retiro { get; set; }
        public ComisionOperacionMS CobroServicios { get; set; }
    }
}