using System;

namespace FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial
{
    public class ModeloCorresponsal
    {
        public int Id { get; set; }

        public ModeloPersona Persona { get; set; }
        
        public DateTime FechaNacimiento { get; set; }
        
        public string Direccion { get; set; }
        
        public float Latitud { get; set; }
        
        public float Longitud { get; set; }
        
        public ModeloSupervisor Supervisor { get; set; }
        
        public bool EstaActivo { get; set; }
    }
}
