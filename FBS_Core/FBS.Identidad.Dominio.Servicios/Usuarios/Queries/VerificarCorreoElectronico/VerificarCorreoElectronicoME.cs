using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class VerificarCorreoElectronicoME : IRequest<bool>
    {
        public string CorreoElectronico { get; set; }
        public string IdUsuario { get; set; }
    }
}