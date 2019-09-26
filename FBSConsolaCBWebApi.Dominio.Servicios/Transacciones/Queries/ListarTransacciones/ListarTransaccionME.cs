using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionME : IRequest<ListarTransaccionMS>
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public IEnumerable<ModeloFiltro> Filtros { get; set; }

        public IEnumerable<ModeloOrdenamiento> Ordenamientos { get; set; }
    }
}