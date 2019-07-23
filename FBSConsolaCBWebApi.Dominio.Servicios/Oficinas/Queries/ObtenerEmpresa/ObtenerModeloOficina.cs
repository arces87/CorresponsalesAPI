using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries
{
    public class ObtenerModeloOficina
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ciudad { get; set; }
        public int IdEmpresa { get; set; }
        public bool EstaActivo { get; set; }
        public string NombreEmpresa { get; set; }
    }
}
