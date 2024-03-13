using AutoMapper;
using FBS.DAL.Nomenclador;
using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class ModificarUsuarioHandler : IRequestHandler<ModificarUsuarioME, string>
    {
        private readonly IRepositorioRol _repositorioRol;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly byte[] _llave;
        private readonly IUsuarioApi _clienteApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IGeneralesApi _envioSMSApi;

        public ModificarUsuarioHandler(
            IRepositorioRol repositorioRol, 
            IMediator mediador,
            UserManager<Usuario> manejadorUsuario, 
            IMapper mapper,
            IUsuarioApi clienteApi,
            IApiKeyGenerator apiKeyGenerator,
            IRepositorioUsuario repositorioUsuario,
            IGeneralesApi envioSMSApi)
        {
            _repositorioRol = repositorioRol;
            _mediador = mediador;
            _manejadorUsuario = manejadorUsuario;
            _mapper = mapper;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _clienteApi = clienteApi;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioUsuario = repositorioUsuario;
            _envioSMSApi = envioSMSApi;
        }

        public async Task<string> Handle(ModificarUsuarioME request, CancellationToken cancellationToken)
        {
            var usuario = _manejadorUsuario.Users.FirstOrDefault(u => u.Id == request.Id);
            var emailActual = usuario.Email;
            var cambioEmailUsuario = CambioEmailUsuario(request, usuario);
            var cambioContrasenia = CambioContrasenia(request);
            var cambioMovilUsuario = CambioMovilUsuario(request, usuario);

            ManejaCambioContrasenia(request, usuario, cambioContrasenia);
            var usuarioMapeado = _mapper.Map<Usuario>(request);
            MapeaDatosUsuarios(request, usuario);
            await _manejadorUsuario.UpdateAsync(usuario);
            var _nuevosRoles = await AsignarRoles(request.Roles, usuario);
            await EliminarRoles(usuario, _nuevosRoles);
            usuario.Operadora = usuarioMapeado.Operadora;
            await _repositorioUsuario.Update(usuario);
            await NotificarCambioDatosUsuario(request, emailActual, cambioEmailUsuario, cambioContrasenia, cambioMovilUsuario);
            return usuario.Id;
        }
        
        private async Task NotificarCambioDatosUsuario(ModificarUsuarioME request, string emailActual, bool cambioEmailUsuario, bool cambioContrasenia, bool cambioMovilUsuario)
        {
            try
            { 
                var emailTemplate = File.ReadAllText("Resources/EmailTemplate/cambio_credenciales.html");
                var fechaActual = DateTime.Now.ToString("dd/MM/yyyy H:mm");

                if (cambioContrasenia)
                {
                    emailTemplate = emailTemplate.Replace("[:NOMBREUSUARIO:]", request.Usuario)
                        .Replace("[:CONTRASENIA:]", request.Contrasenna)
                        .Replace("[:NOMBRE:]", request.NombreMostrar)
                        .Replace("[:TELEFONO:]", request.Telefono);

                    var plantillaSMS = File.ReadAllText("Resources/SmsTemplate/template_cambio_clave.txt");
                    plantillaSMS = plantillaSMS.Replace("[:NOMBREUSUARIO:]", request.Usuario)
                            .Replace("[:FECHA:]", fechaActual);

                    var mensajeSMS = new EnvioSmsRequest()
                    {
                        CodigoUsuarioCorresponsal = request.Usuario,
                        MensajeTexto = plantillaSMS,
                        NumeroIdentificacion = "",
                        SecuencialTipoIdentificacion = 0,
                        NumeroCelular = request.Telefono
                    };

                    var respuesta = await _envioSMSApi.EnvioSmsAsync(mensajeSMS);
                }
                else 
                {
                    emailTemplate = emailTemplate.Replace("[:NOMBREUSUARIO:]", request.Usuario)
                       .Replace("[:CONTRASENIA:]", "*******")
                       .Replace("[:NOMBRE:]", request.NombreMostrar)
                       .Replace("[:TELEFONO:]", request.Telefono);
                }                

                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = "Modificación de datos del usuario en la Consola de Administración de Corresponsales Solidarios",
                    Mensaje = emailTemplate,
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = request.CorreoElectronico,
                            Nombre = request.NombreMostrar
                        }
                    }
                });

                if (cambioEmailUsuario)
                {
                    var plantillaSMS = File.ReadAllText("Resources/SmsTemplate/template_cambio_correo.txt");
                    plantillaSMS = plantillaSMS.Replace("[:NOMBREUSUARIO:]", request.Usuario)
                            .Replace("[:FECHA:]", fechaActual);

                    var mensajeSMS = new EnvioSmsRequest()
                    {
                        CodigoUsuarioCorresponsal = request.Usuario,
                        MensajeTexto = plantillaSMS,
                        NumeroIdentificacion = "",
                        SecuencialTipoIdentificacion = 0,
                        NumeroCelular = request.Telefono
                    };

                    var respuesta = await _envioSMSApi.EnvioSmsAsync(mensajeSMS);
                }

                if (cambioMovilUsuario)
                {
                    var plantillaSMS = File.ReadAllText("Resources/SmsTemplate/template_cambio_celular.txt");
                    plantillaSMS = plantillaSMS.Replace("[:NOMBREUSUARIO:]", request.Usuario)
                            .Replace("[:FECHA:]", fechaActual);

                    var mensajeSMS = new EnvioSmsRequest()
                    {
                        CodigoUsuarioCorresponsal = request.Usuario,
                        MensajeTexto = plantillaSMS,
                        NumeroIdentificacion = "",
                        SecuencialTipoIdentificacion = 0,
                        NumeroCelular = request.Telefono
                    };

                    var respuesta = await _envioSMSApi.EnvioSmsAsync(mensajeSMS);
                }
            }
            catch (Exception)
            {                 
                throw new ExcepcionApp("Los datos del usuario fueron actualizados correctamente, pero no pudo ser posible notificar al usuario.", TipoError.EmailNotification);
            }
            
        }

        private void ManejaCambioContrasenia(ModificarUsuarioME request, Usuario usuario, bool cambioContrasenia)
        {
            if (cambioContrasenia)
            {                
                var _password = request.Contrasenna;
                usuario.PasswordHash = _manejadorUsuario.PasswordHasher.HashPassword(usuario, _password);
                usuario.FechaUltimoCambioContrasenia = DateTime.Now;
                usuario.CambioContrasenia = false;
            }
        }

        private static void MapeaDatosUsuarios(ModificarUsuarioME request, Usuario usuario)
        {
            usuario.UserName = request.Usuario;
            usuario.Email = request.CorreoElectronico;
            usuario.Imagen = request.Imagen;
            usuario.NombreCompleto = request.NombreCompleto;
            usuario.NombreMostrar = request.NombreMostrar;
            usuario.PhoneNumber = request.Telefono;
        }

        private async Task EliminarRoles(Usuario usuario, List<string> _nuevosRoles)
        {
            var roles = await _manejadorUsuario.GetRolesAsync(usuario);
            foreach (var item in roles)
            {
                var fly = false;
                if (_nuevosRoles.Count != 0 && _nuevosRoles.Contains(item.ToUpper()))
                    fly = true;
                if (!fly)
                {
                    var role = await _repositorioRol.GetForName(item);

                    await _manejadorUsuario.RemoveFromRoleAsync(usuario, role.NormalizedName);
                }
            }
        }

        private async Task<List<string>> AsignarRoles(IEnumerable<ModificarUsuarioRol> roles, Usuario usuario)
        {
            List<string> _nuevosRoles = new List<string>();
            if (roles != null)
            {
                foreach (var item in roles)
                {
                    var rol = await _repositorioRol.Get(item.Id);
                    if (rol != null)
                    {
                        _nuevosRoles.Add(rol.NormalizedName);
                        var existe = await _manejadorUsuario.IsInRoleAsync(usuario, rol.NormalizedName);
                        if (!existe)
                            await _manejadorUsuario.AddToRoleAsync(usuario, rol.NormalizedName);
                    }
                }
            }

            return _nuevosRoles;
        }

        private static bool CambioContrasenia(ModificarUsuarioME request)
        {
            return request.Contrasenna != null && request.Contrasenna != "";
        }

        private static bool TieneRolAgente(IEnumerable<ModificarUsuarioRol> roles)
        {
            return roles != null && roles.Any(r => r.Id == "e0aee3b1-57c9-4d4e-8f35-4bf3bc38236e");
        }

        private static bool CambioEmailUsuario(ModificarUsuarioME request, Usuario _user)
        {
            return _user.Email.ToUpper() != request.CorreoElectronico.ToUpper();
        }

        private static bool CambioMovilUsuario(ModificarUsuarioME request, Usuario _user)
        {
            return _user.PhoneNumber != request.Telefono;
        }
    }
}
