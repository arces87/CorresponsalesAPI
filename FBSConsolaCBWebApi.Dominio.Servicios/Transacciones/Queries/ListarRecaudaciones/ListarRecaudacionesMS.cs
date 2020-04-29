namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarRecaudacionesMS
    {
        public double MontoCaja { get; set; }
        public ModeloListaRecaudaciones Deposito { get; set; }
        public ModeloListaRecaudaciones Retiro { get; set; }
        public ModeloListaRecaudaciones CobroServicios { get; set; }
        public ModeloListaRecaudaciones AbonoPrestamo { get; set; }
    }
}
