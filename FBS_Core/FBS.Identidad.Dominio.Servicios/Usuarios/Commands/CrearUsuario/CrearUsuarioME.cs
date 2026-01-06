using MediatR;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class CrearUsuarioME : IRequest<string>
    {
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenna { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreMostrar { get; set; }
        public string Telefono { get; set; }
        public string Imagen { get; set; }
        public string IdOperadora { get; set; }
        public IEnumerable<CrearUsuarioRol> Roles { get; set; }
        public int Cuenta { get; set; }
    }
}
