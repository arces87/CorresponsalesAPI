using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries
{
    public class ObtenerLimiteExistenciaQuery : IRequest<ObtenerModeloLimiteExistencia>
    {
        public int Id { get; set; }
    }
}