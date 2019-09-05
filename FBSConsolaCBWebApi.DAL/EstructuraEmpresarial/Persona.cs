using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.EstructuraEmpresarial
{
    [Table("Persona", Schema = "EstructuraEmpresarial")]
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string NombreUnido { get; set; }

        public int NumeroIdentificador { get; set; }

        public string Identificacion { get; set; }

        public string Direccion { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        public bool EstaActivo { get; set; }
        public string ImagenUrl { get; set; }

        public Catalogo TipoIdentificacion { get; set; }

        public Oficina Oficina { get; set; }

        public Usuario Usuario { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }


    }
}
