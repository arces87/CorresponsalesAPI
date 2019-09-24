using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerLogQuery : IRequest<ObtenerModeloLog>
    {
        public string Id { get; set; }
    }
}