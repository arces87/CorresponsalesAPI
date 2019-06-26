using FBS_Core.Identity.DAL.Seguridad;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial
{
    [Table("SUPERVISOR", Schema = "ESTRUCTURAEMPRESARIAL")]
    public class Supervisor
    {
        [Key]
        [ForeignKey(nameof(Persona))]
        [Column("PERSONAID")]
        public int Id { get; set; }

        public Persona Persona { get; set; }

        public IEnumerable<Corresponsal> Corresponsales { get; set; }

        [Column("ESTAACTIVO")]
        public bool EstaActivo { get; set; }

        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }


    }
}
