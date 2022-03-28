using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerListaLogQuery : IRequest<ModeloObtenerListaLog>
    {
        public int IdCorresponsal { get; set; }
    }
}