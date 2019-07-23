using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerLogQuery : IRequest<ObtenerModeloLog>
    {
        public int Id { get; set; }
    }
}