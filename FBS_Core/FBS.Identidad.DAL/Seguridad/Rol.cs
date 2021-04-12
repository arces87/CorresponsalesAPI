using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FBS.Identidad.DAL.Seguridad
{
    public class Rol: IdentityRole
    {
        public string Descripcion { get; set; }
        public IEnumerable<RolMenu> RoleMenu { get; set; }
        public bool EstaActivo { get; set; }
    }
}
