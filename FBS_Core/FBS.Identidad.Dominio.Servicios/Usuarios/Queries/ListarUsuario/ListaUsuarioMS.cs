using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ListaUsuarioMS
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public int TotalElementos { get; set; }
        public List<ModeloListaUsuario> Usuarios { get; set; }
    }
}
