using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class ModeloLoginUsuario
    {
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string NombreMostrar { get; set; }
        public string Imagen { get; set; }
        public bool CambioContrasenia { get; set; }
        public string CorreoElectronico { get; set; }
        public string Errores { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<LoginUsuarioRol> Roles { get; set; }        
        public string TelefonoCelular { get; set; }
    }
}
