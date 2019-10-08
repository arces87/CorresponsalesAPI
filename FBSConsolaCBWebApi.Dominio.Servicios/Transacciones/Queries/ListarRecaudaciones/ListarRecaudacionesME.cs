using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarRecaudacionesME : IRequest<ListarRecaudacionesMS>
    {
        public string IdAgente { get; set; }
    }
}