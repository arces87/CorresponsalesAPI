using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionAgenteME : IRequest<ListarTransaccionAgenteMS>
    {

        public string IdAgente { get; set; }
        public string IdTipoTransaccion { get; set; }
    }
}