using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;

namespace FBSConsolaCB_WebApi.Dominio.Modelos.Consola
{
    public class ModeloLimiteExistencia
    {
        public int Id { get; set; }

        public double Limite { get; set; }

        public ModeloCorresponsal Corresponsal { get; set; }
        
        public bool EstaActivo { get; set; }
    }
}
