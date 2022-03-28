using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class CambioContraseniaHandler : IRequestHandler<CambioContraseniaME, string>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioUsuario _repositorio;
        private readonly byte[] _llave;

        public CambioContraseniaHandler(UserManager<Usuario> manejadorUsuario, IRepositorioUsuario repositorio)
        {
            _manejadorUsuario = manejadorUsuario;
            _repositorio = repositorio;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<string> Handle(CambioContraseniaME request, CancellationToken cancellationToken)
        {
            var comprobacion = await _repositorio.ComprobarTokenRecuperarContrasenia(request.Token);
            if (comprobacion != null)
            {
                var usuario = _manejadorUsuario.Users.FirstOrDefault(u => u.Id == comprobacion);
                //var _password = string.Format(Criptografia.DecryptPassword(request.Contrasenia, _llave, _llave));
                var _password = request.Contrasenia;
                usuario.PasswordHash = _manejadorUsuario.PasswordHasher.HashPassword(usuario, _password);
                usuario.FechaUltimoCambioContrasenia = DateTime.Now;
                usuario.CambioContrasenia = false;
                await _manejadorUsuario.UpdateAsync(usuario);
                await _repositorio.EliminarTokenRecuperarContrasenia(usuario.Id.ToString());
                return "OK";
            }
            else
                throw new ExcepcionApp("No hay un proceso de recuperación con ese Token");

        }
    }
}
