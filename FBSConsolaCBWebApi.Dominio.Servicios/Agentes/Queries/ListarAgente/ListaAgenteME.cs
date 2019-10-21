using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaAgenteME : IRequest<ListaAgenteMS>
    {
        public string IdSupervisor { get; set; }
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public IEnumerable<ModeloFiltro> Filtros { get; set; }

        public IEnumerable<ModeloOrdenamiento> Ordenamientos { get; set; }
    }
}