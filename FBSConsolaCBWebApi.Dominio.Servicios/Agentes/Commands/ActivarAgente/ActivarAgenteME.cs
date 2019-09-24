using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class ActivarAgenteME : IRequest<string>
    {
        public string IdAgente { get; set; }
    }
}
