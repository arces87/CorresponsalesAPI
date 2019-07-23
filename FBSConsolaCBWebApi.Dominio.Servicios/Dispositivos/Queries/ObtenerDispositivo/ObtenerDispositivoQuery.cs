using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerDispositivoQuery : IRequest<ObtenerModeloDispositivo>
    {
        public int Id { get; set; }
    }
}