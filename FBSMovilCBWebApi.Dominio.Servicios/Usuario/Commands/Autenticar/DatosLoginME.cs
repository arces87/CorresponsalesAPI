using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class DatosLoginME : IRequest<ProcesarLoginMS>
    {
        public UsuarioME User { get; set; }
        public int NumeroIntento { get; set; }
    }
}
