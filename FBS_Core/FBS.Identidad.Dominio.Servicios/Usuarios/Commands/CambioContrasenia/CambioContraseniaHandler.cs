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
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class CambioContraseniaHandler : IRequestHandler<CambioContraseniaME, string>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioUsuario _repositorio;
        private readonly IRepositorioRol _repositorioRol;
        private readonly IUsuarioApi _clienteApi;
        private readonly byte[] _llave;

        public CambioContraseniaHandler(UserManager<Usuario> manejadorUsuario, IRepositorioUsuario repositorio, IRepositorioRol repositorioRol, IUsuarioApi clienteApi)
        {
            _manejadorUsuario = manejadorUsuario;
            _repositorio = repositorio;
            _repositorioRol = repositorioRol;
            _clienteApi = clienteApi;
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
                
                // Llamar al servicio de cambio de clave del core financiero si el usuario es Agente
                await CambiarClaveCoreFinanciero(usuario, request.ContraseniaAnterior, request.Contrasenia);
                
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

        private async Task CambiarClaveCoreFinanciero(Usuario usuario,string claveAnterior, string nuevaClave)
        {
            var roles = await _manejadorUsuario.GetRolesAsync(usuario);
            if (roles != null && roles.Any())
            {
                foreach (var rolNombre in roles)
                {
                    var rol = await _repositorioRol.GetForName(rolNombre);
                    if (rol != null && rol.Id == "e0aee3b1-57c9-4d4e-8f35-4bf3bc38236e") // Rol Agente
                    {
                        try
                        {
                            var respuesta = await _clienteApi.CambiaClaveCorresponsalesAsync(
                                new CambiaClaveCorresponsalesRequest()
                                {
                                    CodigoUsuario = usuario.UserName,
                                    AnteriorClave = claveAnterior,
                                    NuevaClave = nuevaClave,
                                    NumeroIntento = 1
                                });

                            if (respuesta != null && !respuesta.Estado)
                            {
                                throw new ExcepcionApp(respuesta.MensajeError ?? "No se ha podido cambiar la clave en core financiero, por favor inténtelo más tarde.", TipoError.Error);
                            }
                        }
                        catch (Exception e)
                        {
                            throw new ExcepcionApp("No se ha podido cambiar la clave en core financiero, por favor inténtelo más tarde.", TipoError.Error);
                        }
                        break; // Solo llamar una vez
                    }
                }
            }
        }
    }
}
