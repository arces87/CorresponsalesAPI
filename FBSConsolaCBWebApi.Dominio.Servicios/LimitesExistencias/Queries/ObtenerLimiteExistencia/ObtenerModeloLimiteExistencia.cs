namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries
{
    public class ObtenerModeloLimiteExistencia
    {
        public int Id { get; set; }
        public double Limite { get; set; }
        public int IdCorresponsal { get; set; }
        public string NombreCorresponsal { get; set; }
        public bool EstaActivo { get; set; }
    }
}
