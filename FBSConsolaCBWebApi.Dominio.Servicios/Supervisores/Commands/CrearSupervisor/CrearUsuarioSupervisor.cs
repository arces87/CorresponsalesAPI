using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class CrearUsuarioSupervisor
    {
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenna { get; set; }
        public IEnumerable<CrearRolSupervisor> Roles { get; set; }
    }
}
