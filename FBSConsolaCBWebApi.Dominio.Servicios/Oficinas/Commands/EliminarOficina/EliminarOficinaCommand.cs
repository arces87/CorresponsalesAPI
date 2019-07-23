using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Oficinas.Commands
{
    public class EliminarOficinaCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
