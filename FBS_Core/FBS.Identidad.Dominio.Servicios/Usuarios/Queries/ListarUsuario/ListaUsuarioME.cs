using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ListaUsuarioME : IRequest<ListaUsuarioMS>
    {
        public bool? Activo { get; set; }
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public IEnumerable<ModeloFiltro> Filtros { get; set; }

        public IEnumerable<ModeloOrdenamiento> Ordenamientos { get; set; }
    }
}