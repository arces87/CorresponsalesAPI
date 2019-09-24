using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ListarAlertaME : IRequest<ListarAlertaMS>
    {
        public int CantidadElementos { get; set; }
    }
}