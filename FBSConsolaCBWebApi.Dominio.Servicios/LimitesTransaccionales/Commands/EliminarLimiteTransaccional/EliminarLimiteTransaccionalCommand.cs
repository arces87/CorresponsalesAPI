using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands
{
    public class EliminarLimiteTransaccionalCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
