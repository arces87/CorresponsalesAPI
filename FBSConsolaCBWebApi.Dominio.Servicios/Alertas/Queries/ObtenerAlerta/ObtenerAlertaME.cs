using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerAlertaME : IRequest<ObtenerAlertaMS>
    {
        public string Id { get; set; }
    }
}