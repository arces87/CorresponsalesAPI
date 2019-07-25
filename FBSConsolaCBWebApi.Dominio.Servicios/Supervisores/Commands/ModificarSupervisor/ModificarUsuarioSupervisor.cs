using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class ModificarUsuarioSupervisor
    {
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenna { get; set; }
        public IEnumerable<ModificarRolSupervisor> Roles { get; set; }
    }
}
