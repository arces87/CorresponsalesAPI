using FBS_Core.Identity.DAL.Seguridad;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial
{
    [Table("CORRESPONSAL", Schema = "ESTRUCTURAEMPRESARIAL")]
    public class Corresponsal
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("PRIMERNOMBRE")]
        public string PrimerNombre { get; set; }

        [Column("SEGUNDONOMBRE")]
        public string SegundoNombre { get; set; }

        [Column("PRIMERAPELLIDO")]
        public string PrimerApellido { get; set; }

        [Column("SEGUNDOAPELLIDO")]
        public string SegundoApellido { get; set; }

        [Column("NOMBREUNIDO")]
        public string NombreUnido { get; set; }

        [Column("DIRECCION")]
        public string Direccion { get; set; }

        [Column("NUMEROIDENTIFICADOR")]
        public int NumeroIdentificador { get; set; }

        [Column("IDENTIFICACION")]
        public string Identificacion { get; set; }

        [Column("CORREOELECTRONICO")]
        public string Email { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

        [Column("FECHANACIMIENTO")]
        public DateTime FechaNacimiento { get; set; }

        [Column("ESTAACTIVO")]
        public bool EstaActivo { get; set; }
        
        [ForeignKey("TIPOIDENTIFICACIONID")]
        public Catalogo TipoIdentificacion { get; set; }

        [ForeignKey("CARGOID")]
        public Cargo Cargo { get; set; }

        [ForeignKey("OFICINAID")]
        public Oficina Oficina { get; set; }

        [ForeignKey("AREATRABAJOID")]
        public AreaTrabajo AreaTrabajo { get; set; }

        [ForeignKey("USUARIOID")]
        public User Usuario { get; set; }


        [Column("CONCURRENCIA")]
        [Timestamp]
        public byte[] RowVersion { get; set; }
        

    }
}
