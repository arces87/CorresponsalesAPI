using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerPersonaQuery : IRequest<ObtenerModeloPersona>
    {
        public int Id { get; set; }
    }
}