using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;

namespace FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial
{
    public class ModeloPersona
    {
        public int Id { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NombreUnido { get; set; }
        public int NumeroIdentificador { get; set; }
        public string Identificacion { get; set; }
        public bool EstaActivo { get; set; }
        public bool Interno { get; set; }
        public ModeloOficina Oficina { get; set; }

        public ModeloCatalogo TipoIdentificacion { get; set; }

        public ModeloUsuario Usuario { get; set; }
    }
}
