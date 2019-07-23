using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class ModificarCatalogoCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoCatalogo { get; set; }
    }
}
