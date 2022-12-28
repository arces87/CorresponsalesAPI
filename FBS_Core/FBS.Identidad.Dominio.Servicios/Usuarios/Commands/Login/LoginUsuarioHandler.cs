using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
    public class LoginUsuarioHandler : IRequestHandler<LoginUsuarioME, ModeloLoginUsuario>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly SignInManager<Usuario> _manejadorAutenticacion;
        private readonly IRepositorioRol _repositorioRol;
        private readonly IRepositorioUsuario _repositorio;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuracion;
        private readonly byte[] _llave;
        private readonly IRepositorioCanal _repositorioCanal;
        private readonly IConfiguracionCanal _configuracionCanal;

        public LoginUsuarioHandler(UserManager<Usuario> manejadorUsuario,
            SignInManager<Usuario> manejadorAutenticacion, IRepositorioRol repositorioRol, IRepositorioUsuario repositorio, IMapper mapper,
            IConfiguration configuracion, IRepositorioCanal repositorioCanal, IConfiguracionCanal configuracionCanal)
        {
            _manejadorUsuario = manejadorUsuario;
            _manejadorAutenticacion = manejadorAutenticacion;
            _repositorioRol = repositorioRol;
            _repositorio = repositorio;
            _mapper = mapper;
            _configuracion = configuracion;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY-+2He.");
            _repositorioCanal = repositorioCanal;
            _configuracionCanal = configuracionCanal;
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

        private async Task<string> GenerateJwtTokenCambioContrasenia(Usuario user)
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
                Expires = DateTime.UtcNow.AddMinutes(_configuracionCanal.Negocio.TiempoVidaToken),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ModeloLoginUsuario> Handle(LoginUsuarioME request, CancellationToken cancellationToken)
        {
            var retorno = new ModeloLoginUsuario();
            var _user = _manejadorUsuario.Users.Where(u => u.UserName == request.Usuario).FirstOrDefault();

            if (_user == null)
            {
                retorno.Errores = "El usuario no se encuentra registrado en el sistema";
                return retorno;
            }            
            
            var result = await _manejadorAutenticacion.PasswordSignInAsync(request.Usuario, request.Contrasenna, false, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                retorno.Errores = "Ha excedido el número máximo de intentos, su cuenta fue bloqueada.";
                _user.EstaActivo = false;
                await _manejadorUsuario.UpdateAsync(_user);
                return retorno;
            }            
            
            if (!result.Succeeded)
            {
                retorno.Errores = "Usuario o contraseña incorrectos.";
                return retorno;
            }

            if (!_user.EstaActivo)
            {
                retorno.Errores = "El usuario se encuentra bloqueado.";
                return retorno;
            }           

            var rolAgente = await _manejadorUsuario.IsInRoleAsync(_user, "AGENTE");

            if (rolAgente && request.Dispositivo == "Consola")
            {
                retorno.Errores = "El usuario con rol Agente no puede autenticarse en el módulo Consola Administrativa";
                return retorno;
            }
            
            if (!rolAgente && request.Dispositivo == "Movil")
            {
                retorno.Errores = "En el móvil solo pueden acceder los Agentes de Corresponsales Solidarios";
                return retorno;
            }

            var canal = await _repositorioCanal.GetCanalUsuario(_user.Id);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(canal.JsonNegocio);
            var contrasennaExpiro = (DateTime.Now - _user.FechaUltimoCambioContrasenia).TotalDays >= jsonNegocio.DiasValidosContrasenna;

            if (_user.CambioContrasenia || contrasennaExpiro)
            {
                var token = await GenerateJwtTokenCambioContrasenia(_user);
                await _repositorio.SalvarTokenRecuperarContrasenia(_user.Id, token);
                retorno.CambioContrasenia = true;
                retorno.IdUsuario = _user.Id;
                retorno.Token = token;
                _user.CambioContrasenia = true;
                await _manejadorUsuario.UpdateAsync(_user);

                if (contrasennaExpiro)
                {
                    retorno.Errores = "Usted debe de cambiar su contraseña porque ha expirado";
                }
                else
                {
                    retorno.Errores = "Usted debe de cambiar su contraseña en el primer acceso";
                }

                return retorno;
            }

            try
            {

                var token = await GenerateJwtToken(_user);
                var roles = await _manejadorUsuario.GetRolesAsync(_user);
                var _roles = new List<LoginUsuarioRol>();
                foreach (var item in roles)
                {
                    var role = _mapper.Map<LoginUsuarioRol>(await _repositorioRol.GetForName(item));
                    _roles.Insert(_roles.Count, role);
                }
                retorno.IdUsuario = _user.Id;
                retorno.Usuario = _user.UserName;
                retorno.CorreoElectronico = _user.Email;
                retorno.Imagen = _user.Imagen;
                retorno.NombreMostrar = _user.NombreMostrar;
                retorno.Roles = _roles;
                retorno.Token = (string)token;
                retorno.TelefonoCelular = _user.PhoneNumber;
            }
            catch (Exception e)
            {
                retorno.Errores = "Ha ocurrido un error durante la autenticación.";
            }

            return retorno;
        }
    }
}
