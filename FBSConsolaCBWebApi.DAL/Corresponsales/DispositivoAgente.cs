using FBS.DAL.Nomenclador;
using FBSConsolaCBWebApi.DAL.Canales;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Corresponsales
{
    [Table("DispositivoAgente", Schema = "Corresponsales")]
    public class DispositivoAgente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid DispositivoId { get; set; }
        public Guid AgenteId { get; set; }
       
    }    
}
