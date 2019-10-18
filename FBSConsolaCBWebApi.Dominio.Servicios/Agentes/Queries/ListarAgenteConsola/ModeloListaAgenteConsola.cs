namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ModeloListaAgenteConsola
    {
        public string Id { get; set; }
        public string NombreAgente { get; set; }
        public string Ubicacion { get; set; }
        public int NumeroTransacciones { get; set; }
        public double ExistenciaCaja { get; set; }
        public double ValorComision { get; set; }
        public int NumeroAlerta { get; set; }
        public double ValorReposicion { get; set; }
        public string Estado { get; set; }
    }
}
