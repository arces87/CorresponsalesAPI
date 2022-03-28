using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class ComprobarCorreoElectronicoME : IRequest<bool>
    {
        public string CorreoElectronico { get; set; }

    }
}
