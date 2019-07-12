using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using System;

namespace FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial
{
    public class ModeloCorresponsal
    {
        public int Id { get; set; }

        public ModeloPersona Persona { get; set; }
        
        public float Latitud { get; set; }
        
        public float Longitud { get; set; }
        
        public ModeloSupervisor Supervisor { get; set; }
        
        public bool EstaActivo { get; set; }

        public ModeloDispositivo Dispositivo { get; set; }

        public bool EstaBloqueado { get; set; }
    }
}
