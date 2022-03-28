using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerCatalogoME : IRequest<ObtenerCatalogoMS>
    {
        public string Id { get; set; }
    }
}