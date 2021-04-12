using FBS.Identidad.DAL.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class ComprobarRolHandler : IRequestHandler<ComprobarRolME, bool>
    {
        private readonly RoleManager<Rol> _manejadorRol;

        public ComprobarRolHandler(RoleManager<Rol> manejadorRol)
        {
            _manejadorRol = manejadorRol;
        }

        public async Task<bool> Handle(ComprobarRolME request, CancellationToken cancellationToken)
        {
            if (request.IdRol != null && request.IdRol != "")
                return await _manejadorRol.Roles.AnyAsync(u => u.NormalizedName == request.Rol.ToUpper() && u.Id!= request.IdRol);
            else
                return await _manejadorRol.Roles.AnyAsync(u => u.NormalizedName == request.Rol.ToUpper());
        }
    }
}
