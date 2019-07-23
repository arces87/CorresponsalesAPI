using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ObtenerTipoCatalogoQuery : IRequest<ObtenerModeloTipoCatalogo>
    {
        public int Id { get; set; }
    }
}