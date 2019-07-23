using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Modelos.Nomenclador;
using System;

namespace FBSConsolaCBWebApi.Dominio.Modelos.Consola
{
    public class ModeloLog
    {
        public int Id { get; set; }

        public string Transaccion { get; set; }

        public DateTime Fecha { get; set; }

        public string Canal { get; set; }

        public string Json { get; set; }

        public ModeloCatalogo Operacion { get; set; }

        public ModeloCorresponsal Corresponsal { get; set; }

        public bool EstaActivo { get; set; }
    }
}
