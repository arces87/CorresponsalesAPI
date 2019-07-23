using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;

namespace FBSConsolaCBWebApi.Dominio.Modelos.Consola
{
    public class ModeloLimiteExistencia
    {
        public int Id { get; set; }

        public double Limite { get; set; }

        public ModeloCorresponsal Corresponsal { get; set; }
        
        public bool EstaActivo { get; set; }
    }
}
