using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerCatalogoQuery : IRequest<ObtenerModeloCatalogo>
    {
        public int Id { get; set; }
    }
}