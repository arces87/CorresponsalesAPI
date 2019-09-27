namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ModeloObtenerComisionTransaccion
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public double Valor { get; set; }
        public ComisionTransaccion Comisiones { get; set; }
    }
}