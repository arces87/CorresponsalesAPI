using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class CrearUsuarioCorresponsal
    {
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenna { get; set; }
        public IEnumerable<CrearRolCorresponsal> Roles { get; set; }
    }
}
