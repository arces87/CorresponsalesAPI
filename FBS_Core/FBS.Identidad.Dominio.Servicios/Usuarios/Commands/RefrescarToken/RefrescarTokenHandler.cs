using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class RefrescarTokenHandler : IRequestHandler<RefrescarTokenME, string>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioUsuario _repositorio;
        private readonly IConfiguration _configuracion;
        private readonly IConfiguracionCanal _configuracionCanal;
        private readonly byte[] _llave;

        public RefrescarTokenHandler(UserManager<Usuario> manejadorUsuario, 
            IRepositorioUsuario repositorio, 
            IConfiguration configuracion, 
            IConfiguracionCanal configuracionCanal)
        {
            _manejadorUsuario = manejadorUsuario;
            _repositorio = repositorio;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _configuracion = configuracion;            
            _configuracionCanal = configuracionCanal;
        }

        public async Task<string> Handle(RefrescarTokenME request, CancellationToken cancellationToken)
        {
            var userId = await _repositorio.ComprobarRefreshToken(request.RefreshToken);
            if (userId != null)
            { 
                var _user = _manejadorUsuario.Users.Where(u => u.Id == userId).FirstOrDefault();
                var token = await GenerateJwtToken(_user);
                return token;
            }
            else
                throw new ExcepcionApp("No hay un proceso de recuperación con ese Token");

        }

        private async Task<string> GenerateJwtToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuracion["JwtKey"]);
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, user.Id) };
            var _roles = await _manejadorUsuario.GetRolesAsync(user);
            foreach (var item in _roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray<Claim>()),
                Expires = DateTime.UtcNow.AddMinutes(_configuracionCanal.Negocio.TiempoVidaToken),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
