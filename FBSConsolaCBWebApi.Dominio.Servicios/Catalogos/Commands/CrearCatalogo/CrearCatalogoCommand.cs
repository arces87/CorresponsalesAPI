using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class CrearCatalogoCommand : IRequest<int>
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoCatalogo { get; set; }
    }
}
