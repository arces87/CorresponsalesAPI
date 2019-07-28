using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries
{
    public class ObtenerListaCorresponsalQuery : IRequest<ModeloObtenerListaCorresponsal>
    {
        public int Pagina { get; set; }

        public int CantidadElementos { get; set; }

        public IEnumerable<ModeloFiltro> Filtros { get; set; }

        public IEnumerable<ModeloOrdenamiento> Ordenamientos { get; set; }
    }
}