using System;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ModeloListaUsuario
    {
        public string Id { get; set; }

        public string Usuario { get; set; }

        public string CorreoElectronico { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreMostrar { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Imagen { get; set; }
        public string IdOperadora { get; set; }
        public bool CambioContrasenia { get; set; }
        public string NombreOperadora { get; set; }
        public bool EstaActivo { get; set; }
    }
}
