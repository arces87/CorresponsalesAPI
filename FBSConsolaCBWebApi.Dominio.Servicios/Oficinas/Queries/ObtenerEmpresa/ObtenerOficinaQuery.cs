using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Queries
{
    public class ObtenerOficinaQuery : IRequest<ObtenerModeloOficina>
    {
        public int Id { get; set; }
    }
}