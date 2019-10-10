using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarRecaudacionesME : IRequest<ListarRecaudacionesMS>
    {
        public string IdAgente { get; set; }
    }
}