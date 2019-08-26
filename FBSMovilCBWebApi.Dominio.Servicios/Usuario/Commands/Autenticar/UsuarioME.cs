using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class UsuarioME
    {
        public string UsuarioLogin { get; set; }
        public string Password { get; set; }
        public Guid GuidID { get; set; }
    }
}
