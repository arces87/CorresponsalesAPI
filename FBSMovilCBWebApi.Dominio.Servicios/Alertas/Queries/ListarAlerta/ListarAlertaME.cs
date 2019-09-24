using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ListarAlertaME : IRequest<ListarAlertaMS>
    {
        public int CantidadElementos { get; set; }
    }
}