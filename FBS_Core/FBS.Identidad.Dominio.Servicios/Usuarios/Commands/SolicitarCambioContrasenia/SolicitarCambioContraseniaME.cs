using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class SolicitarCambioContraseniaME : IRequest<bool>
    {
        public string Usuario { get; set; }
        public string Url { get; set; }
       
    }
}
