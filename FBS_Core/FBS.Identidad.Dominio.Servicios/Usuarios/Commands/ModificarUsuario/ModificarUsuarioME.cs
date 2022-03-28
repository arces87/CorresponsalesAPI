using MediatR;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class ModificarUsuarioME : IRequest<string>
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenna { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreMostrar { get; set; }
        public string Imagen { get; set; }
        public string IdOperadora { get; set; }
        public string Telefono { get; set; }
        public IEnumerable<ModificarUsuarioRol> Roles { get; set; }
    }
}
