using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerAlertaQuery : IRequest<ObtenerModeloAlerta>
    {
        public int Id { get; set; }
    }
}