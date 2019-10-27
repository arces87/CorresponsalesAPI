using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class VerificarIdentificacionAgenteME : IRequest<bool>
    {
        public string Identificacion { get; set; }
    }
}