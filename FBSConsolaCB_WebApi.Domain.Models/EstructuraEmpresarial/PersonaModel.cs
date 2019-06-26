
using FBS_Core.Identity.Domain.Models.Seguridad;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using System;

namespace FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial
{
    public class PersonaModel
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
        public OficinaModel Oficina { get; set; }

        public CatalogoModel TipoIdentificacion { get; set; }

        public UserModel Usuario { get; set; }
    }
}
