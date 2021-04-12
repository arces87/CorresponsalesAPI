using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands
{
    public class ModificarTipoCatalogoME : IRequest<string>
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
