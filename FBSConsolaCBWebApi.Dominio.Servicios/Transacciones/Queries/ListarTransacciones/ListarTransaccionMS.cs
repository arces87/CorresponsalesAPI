using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public double Deposito { get; set; }
        public double Retiro { get; set; }
        public double CobroServicios { get; set; }
        public double Caja { get; set; }
        public List<ModeloListaTransaccion> Transacciones { get; set; }
    }
}
