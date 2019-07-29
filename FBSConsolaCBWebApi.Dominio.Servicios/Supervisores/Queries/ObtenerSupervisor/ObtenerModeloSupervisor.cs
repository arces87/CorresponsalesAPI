using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries
{
    public class ObtenerModeloSupervisor
    {
        public int Id { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NombreUnido { get; set; }
        public int NumeroIdentificador { get; set; }
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public bool EstaActivo { get; set; }
        public string Tipo { get; set; }
        public int IdOficina { get; set; }
        public string NombreOficina { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string NombreTipoIdentificacion { get; set; }
        public string Usuario { get; set; }
        public string IdUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreSupervisor { get; set; }
    }
}
