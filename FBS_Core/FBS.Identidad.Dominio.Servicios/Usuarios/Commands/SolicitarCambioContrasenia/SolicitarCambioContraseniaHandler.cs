using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class SolicitarCambioContraseniaHandler : IRequestHandler<SolicitarCambioContraseniaME, bool>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioUsuario _repositorio;
        private readonly IMediator _mediador;
        private readonly byte[] _llave;

        public SolicitarCambioContraseniaHandler(UserManager<Usuario> manejadorUsuario, IRepositorioUsuario repositorio, IMediator mediador)
        {
            _manejadorUsuario = manejadorUsuario;
            _repositorio = repositorio;
            _mediador = mediador;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY-+2He.");
        }

        private async Task<string> GenerateJwtToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _llave;
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, user.Id) };
            var _roles = await _manejadorUsuario.GetRolesAsync(user);
            foreach (var item in _roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray<Claim>()),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        public async Task<bool> Handle(SolicitarCambioContraseniaME request, CancellationToken cancellationToken)
        {
            var usuario = _manejadorUsuario.Users.FirstOrDefault(u => u.Email == request.Usuario);
            if (usuario != null)
            {
                var token = await GenerateJwtToken(usuario);
                await _repositorio.SalvarTokenRecuperarContrasenia(usuario.Id, token);
                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = "Solicitud de cambio de contraseña en la Consola de Administración de Corresponsales Solidarios",
                    Mensaje = $"<html><head></head>" +
                               "<body lang=EN-US link=\"#0563C1\" vlink=\"#954F72\">" +
                               "<p>Se ha solicitado un cambio de contraseña desde su usuario, para proceder con el cambio acceda al siguiente link: " +
                               "<a target=\"_blank\" href=\"" + request.Url + "/" + token + "\">LINK</a>," +
                               " si no puede acceder al vínculo, copie la siguiente" +
                               " URL y peguela en su navegador web: </p><br/><p>" + request.Url + "/" + token + "</p>" +
                               "<p>Si no ha sido usted el que solicitó el cambio contacte con su Administrador.</p><br/><br/>" +
                               "<p>Sistema Mensajeria<br/><b>Corresponsales Solidarios</b></p></body></html>",
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = usuario.Email,
                            Nombre = usuario.NombreCompleto
                        }
                    }
                });
                return true;
            }
            else
                throw new ExcepcionApp("El usuario no se encuentra registrado en el sistema");
        }
    }
}
