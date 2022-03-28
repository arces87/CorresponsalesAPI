using System.Collections.Generic;

namespace FBS.Dominio.Modelos.Filtro
{
    public class ModeloFuenteDatos<T>
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }

        public IEnumerable<T> Datos { get; set; }
    }
}
