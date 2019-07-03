using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial
{
    public class ModeloSupervisor
    {
        public int Id { get; set; }

        public ModeloPersona Persona { get; set; }

        public IEnumerable<ModeloCorresponsal> Corresponsales { get; set; }

        public bool EstaActivo { get; set; }
    }
}
