using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarOtpME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Identificacion { get; set; }
        public bool ParaAgente { get; set; }
    }
}
