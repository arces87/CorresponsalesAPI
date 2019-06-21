
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial
{
    [Table("EMPRESA", Schema = "ESTRUCTURAEMPRESARIAL")]
    public class Empresa
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DIRECCION")]
        public string Direccion { get; set; }

        [Column("LOGO")]
        public string Logo { get; set; }

        [Column("ESTAACTIVO")]
        public bool EstaActivo { get; set; }

        [Column("RUC")]
        public string Ruc { get; set; }

        [Column("ACTIVIDADECONOMICA")]
        public string ActividadEconomica { get; set; }

        [Column("PROVINCIA")]
        public string Provincia { get; set; }

        [Column("DEPARTAMENTO")]
        public string Departamento { get; set; }

        [Column("DISTRITO")]
        public string Distrito { get; set; }

        [Column("FECHAINICIOACTIVIDAD")]
        public DateTime FechaInicioActividad { get; set; }

        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public IEnumerable<Oficina> Oficinas { get; set; }
    }
}
