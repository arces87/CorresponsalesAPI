using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Commands
{
    public class CrearCatalogoME : IRequest<string>
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string IdTipoCatalogo { get; set; }
    }
}
