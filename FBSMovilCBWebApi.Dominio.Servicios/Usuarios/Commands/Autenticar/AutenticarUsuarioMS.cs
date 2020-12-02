using FBS.Identidad.Dominio.Servicios.Canales.Queries;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioMS
    {
        public string Token { get; set; }
        public string Identificacion { get; set; }
        public bool CambioContrasenia { get; set; }
        public string Estado { get; set; }
        public JsonNegocioMS JsonNegocio { get; set; }
        public ComisionesMS Comisiones { get; set; }
        public string NombreMostrar { get; set; }
        public string ReferenciaUbicacion { get; set; }
        public string TelefonoCelular { get; set; }
    }
}
