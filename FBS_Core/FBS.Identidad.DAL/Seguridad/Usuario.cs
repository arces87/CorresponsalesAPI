using FBS.DAL.Nomenclador;
using Microsoft.AspNetCore.Identity;
using System;

namespace FBS.Identidad.DAL.Seguridad
{
    public class Usuario : IdentityUser
    {
        public Catalogo Operadora { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreMostrar { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Imagen { get; set; }
        public bool CambioContrasenia { get; set; }
        public bool EstaActivo { get; set; }
        public DateTime FechaUltimoCambioContrasenia { get; set; }
        public string PhoneNumber { get; set; }
    }
}
