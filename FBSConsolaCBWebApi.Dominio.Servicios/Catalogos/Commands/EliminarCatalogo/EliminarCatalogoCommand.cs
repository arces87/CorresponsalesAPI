using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class EliminarCatalogoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
