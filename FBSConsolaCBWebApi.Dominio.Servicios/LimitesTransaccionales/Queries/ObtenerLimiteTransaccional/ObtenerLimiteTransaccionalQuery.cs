using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ObtenerLimiteTransaccionalQuery : IRequest<ObtenerModeloLimiteTransaccional>
    {
        public int Id { get; set; }
    }
}