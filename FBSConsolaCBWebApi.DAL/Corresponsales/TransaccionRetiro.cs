using FBS.DAL.Nomenclador;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBSConsolaCBWebApi.DAL.Corresponsales
{
    [Table("TransaccionRetiro", Schema = "Corresponsales")]
    public class TransaccionRetiro
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IdTransaccion { get; set; }       
        public double ValorCaja { get; set; }
        public double FondoNegocio { get; set; }       
    }
}
