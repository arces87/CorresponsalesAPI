using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTipoTransaccionME : IRequest<ListarTipoTransaccionMS>
    {
        public string IdAgente { get; set; }
    }
}