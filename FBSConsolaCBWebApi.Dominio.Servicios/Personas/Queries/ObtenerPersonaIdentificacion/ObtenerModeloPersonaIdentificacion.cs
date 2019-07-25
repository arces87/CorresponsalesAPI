using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerModeloPersonaIdentificacion
    {
        public int Id { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NombreUnido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
