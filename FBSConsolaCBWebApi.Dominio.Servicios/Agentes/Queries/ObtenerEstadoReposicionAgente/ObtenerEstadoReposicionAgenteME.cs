using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerEstadoReposicionAgenteME: IRequest<ObtenerEstadoReposicionAgenteMS>
    {
        public string AgenteId { get; set; }
    }
}
