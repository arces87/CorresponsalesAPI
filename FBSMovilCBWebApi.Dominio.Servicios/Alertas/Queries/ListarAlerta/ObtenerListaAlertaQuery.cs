using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerListaAlertaQuery : IRequest<ModeloObtenerListaAlerta>
    {
        public int IdDestinatario { get; set; }
    }
}