namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ModeloObtenerDetalleListaLimiteTransaccional
    {
        public int Id { get; set; }
        public double Monto { get; set; }
        public int Dias { get; set; }
        public int IdCorresponsal { get; set; }
        public string NombreCorresponsal { get; set; }
        public int IdOperacion { get; set; }
        public string NombreOperacion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
