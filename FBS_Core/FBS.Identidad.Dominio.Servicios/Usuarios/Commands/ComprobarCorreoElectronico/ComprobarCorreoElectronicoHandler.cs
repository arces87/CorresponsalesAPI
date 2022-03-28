using FBS.Identidad.DAL.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class ComprobarCorreoElectronicoHandler : IRequestHandler<ComprobarCorreoElectronicoME, bool>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;

        public ComprobarCorreoElectronicoHandler(UserManager<Usuario> manejadorUsuario)
        {
            _manejadorUsuario = manejadorUsuario;
        }

        public async Task<bool> Handle(ComprobarCorreoElectronicoME request, CancellationToken cancellationToken)
        {
            var usuario = await _manejadorUsuario.Users.FirstOrDefaultAsync(u => u.Email == request.CorreoElectronico);
            if (usuario != null)
                return false;
            else
                return true;
        }
    }
}
