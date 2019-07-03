
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial
{
    [Table("Empresa", Schema = "EstructuraEmpresarial")]
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Logo { get; set; }

        public bool EstaActivo { get; set; }

        public string Ruc { get; set; }

        public string ActividadEconomica { get; set; }

        public string Provincia { get; set; }

        public string Departamento { get; set; }

        public string Distrito { get; set; }

        public DateTime FechaInicioActividad { get; set; }

        [Timestamp]
        public byte[] Concurrencia { get; set; }

        public IEnumerable<Oficina> Oficinas { get; set; }
    }
}
