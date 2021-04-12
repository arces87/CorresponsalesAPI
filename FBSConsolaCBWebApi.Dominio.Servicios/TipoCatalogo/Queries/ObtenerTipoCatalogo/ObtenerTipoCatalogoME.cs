using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ObtenerTipoCatalogoME : IRequest<ObtenerTipoCatalogoMS>
    {
        public string Id { get; set; }
    }
}