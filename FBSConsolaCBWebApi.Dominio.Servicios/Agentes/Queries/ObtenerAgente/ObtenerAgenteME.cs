using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerAgenteME : IRequest<ObtenerAgenteMS>
    {
        public string Id { get; set; }
    }
}