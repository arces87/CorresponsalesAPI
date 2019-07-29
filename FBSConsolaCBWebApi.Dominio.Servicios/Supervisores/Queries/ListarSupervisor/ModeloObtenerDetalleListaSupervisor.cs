namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries
{
    public class ModeloObtenerDetalleListaSupervisor
    {
        public int Id { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NombreUnido { get; set; }
        public int NumeroIdentificador { get; set; }
        public string Identificacion { get; set; }
        public int IdOficina { get; set; }
        public string NombreOficina { get; set; }
    }
}
