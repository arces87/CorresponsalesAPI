using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Commands
{
    public class ModificarTipoCatalogoCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
