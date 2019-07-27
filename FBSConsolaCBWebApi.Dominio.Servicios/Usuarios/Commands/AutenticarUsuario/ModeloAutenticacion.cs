using System;
using System.Collections.Generic;
using System.Text;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class ModeloAutenticacion
    {
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Errores { get; set; }
        public string Token { get; set; }
        public IEnumerable<ModeloRolAutenticacion> Roles { get; set; }
        public int IdPersona { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Identificacion { get; set; }
        public int IdOficina { get; set; }
        public string NombreOficina { get; set; }
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
    }
}
