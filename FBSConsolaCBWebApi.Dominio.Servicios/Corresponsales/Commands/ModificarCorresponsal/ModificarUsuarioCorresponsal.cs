using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class ModificarUsuarioCorresponsal
    {
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenna { get; set; }
        public IEnumerable<ModificarRolCorresponsal> Roles { get; set; }
    }
}
