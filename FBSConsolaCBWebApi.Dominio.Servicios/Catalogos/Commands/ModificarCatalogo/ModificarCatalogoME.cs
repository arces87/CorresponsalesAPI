using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class ModificarCatalogoME : IRequest<string>
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string IdTipoCatalogo { get; set; }
    }
}
