using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Dominio.Modelos.Nomenclador;

namespace FBSConsolaCBWebApi.Dominio.Modelos.Consola
{
    public class ModeloLimiteTransaccional
    {
        public int Id { get; set; }

        public double Monto { get; set; }

        public int Dias { get; set; }

        public ModeloCorresponsal Corresponsal { get; set; }

        public ModeloCatalogo Operacion { get; set; }

        public bool EstaActivo { get; set; }
    }
}
