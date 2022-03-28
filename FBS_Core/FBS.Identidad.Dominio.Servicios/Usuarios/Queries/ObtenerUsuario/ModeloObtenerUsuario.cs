using System;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ModeloObtenerUsuario
    {
        public string Id { get; set; }

        public string Usuario { get; set; }

        public string CorreoElectronico { get; set; }

        public string Token { get; set; }

        public string NombreCompleto { get; set; }
        public string NombreMostrar { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Imagen { get; set; }
        public string Telefono { get; set; }
        public string IdOperadora { get; set; }
        public string NombreOperadora { get; set; }
        public IEnumerable<ObtenerUsuarioRol> Roles { get; set; }
    }
}
