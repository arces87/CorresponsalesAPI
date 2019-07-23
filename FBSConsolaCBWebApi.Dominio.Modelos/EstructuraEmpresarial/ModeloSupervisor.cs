namespace FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial
{
    public class ModeloSupervisor
    {
        public int Id { get; set; }

        public ModeloPersona Persona { get; set; }
        
        public bool EstaActivo { get; set; }
    }
}
