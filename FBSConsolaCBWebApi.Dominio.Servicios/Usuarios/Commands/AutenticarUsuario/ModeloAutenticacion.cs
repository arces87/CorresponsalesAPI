using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class ModeloAutenticacion
    {
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string NombreMostrar { get; set; }
        public string Imagen { get; set; }
        public string CorreoElectronico { get; set; }
        public string Errores { get; set; }
        public string Token { get; set; }
        public IEnumerable<ModeloRolAutenticacion> Roles { get; set; }
        public int IdAgente { get; set; }
    }
}
