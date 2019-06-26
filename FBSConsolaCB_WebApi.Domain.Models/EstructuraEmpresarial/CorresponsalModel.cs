using System;

namespace FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial
{
    public class CorresponsalModel
    {
        public int Id { get; set; }

        public PersonaModel Persona { get; set; }
        
        public DateTime FechaNacimiento { get; set; }
        
        public string Direccion { get; set; }
        
        public float Latitud { get; set; }
        
        public float Longitud { get; set; }
        
        public SupervisorModel Supervisor { get; set; }
        
        public bool EstaActivo { get; set; }
    }
}
