using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class EliminarCatalogoME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
