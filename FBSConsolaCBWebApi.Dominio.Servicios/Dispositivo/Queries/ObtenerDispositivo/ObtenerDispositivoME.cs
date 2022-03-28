using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerDispositivoME : IRequest<ObtenerDispositivoMS>
    {
        public string Id { get; set; }
    }
}