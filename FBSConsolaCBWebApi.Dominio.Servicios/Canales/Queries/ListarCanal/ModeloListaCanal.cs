namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Queries
{
    public class ModeloListaCanal
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string JsonNegocio { get; set; }

        public string JsonConfiguracion { get; set; }

        public bool EstaActivo { get; set; }
    }
}
