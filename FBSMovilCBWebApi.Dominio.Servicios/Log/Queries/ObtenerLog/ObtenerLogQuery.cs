using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerLogQuery : IRequest<ObtenerModeloLog>
    {
        public int Id { get; set; }
    }
}