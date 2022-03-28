using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class EliminarAgenteME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
