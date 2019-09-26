using FBS.Dominio.Modelos.Filtro;
using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ObtenerComisionTransaccionME : IRequest<ObtenerComisionTransaccionMS>
    {
        public string IdAgente { get; set; }
    }
}