using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioMS
    {
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string NombreMostrar { get; set; }
        public string Imagen { get; set; }
        public string CorreoElectronico { get; set; }
        public bool CambioContrasenia { get; set; }
        public string Errores { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<AutenticarUsuarioRol> Roles { get; set; }
        public string IdAgente { get; set; }
    }
}
