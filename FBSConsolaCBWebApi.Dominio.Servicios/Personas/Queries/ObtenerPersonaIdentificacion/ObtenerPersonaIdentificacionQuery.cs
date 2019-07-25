using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerPersonaIdentificacionQuery : IRequest<ObtenerModeloPersonaIdentificacion>
    {
        public string Identificacion { get; set; }
    }
}