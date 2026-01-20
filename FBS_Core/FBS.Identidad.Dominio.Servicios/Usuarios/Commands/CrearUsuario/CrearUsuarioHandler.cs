using AutoMapper;
using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
    public class CrearUsuarioHandler : IRequestHandler<CrearUsuarioME, string>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioRol _repositorioRol;
        private readonly IRepositorioUsuario _repositorio;
        private readonly IConfiguration _configuracion;
        private readonly IUsuarioApi _clienteApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly byte[] _llave;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public CrearUsuarioHandler(
            UserManager<Usuario> manejadorUsuario,
            IRepositorioRol repositorioRol,
            IUsuarioApi clienteApi,
            IRepositorioUsuario repositorio,
            IMapper mapper,
            IConfiguration configuracion,
            IMediator mediador,
            IApiKeyGenerator apiKeyGenerator)
        {
            _manejadorUsuario = manejadorUsuario;
            _repositorio = repositorio;
            _repositorioRol = repositorioRol;
            _clienteApi = clienteApi;
            _mapper = mapper;
            _configuracion = configuracion;
            _mediador = mediador;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<string> Handle(CrearUsuarioME request, CancellationToken cancellationToken)
        {
            var _user = _mapper.Map<Usuario>(request);

            await GenerarUsuarioCoreFinanciero(request, _user);

            var _password = request.Contrasenna;
            var operadora = _user.Operadora;
            _user.Operadora = null;
            _user.FechaCreacion = DateTime.UtcNow;
            _user.CambioContrasenia = true;
            _user.FechaUltimoCambioContrasenia = DateTime.Now;
            var result = await _manejadorUsuario.CreateAsync(_user, _password);

            if (result.Succeeded)
            {
                await AsignarRoles(request, _user);
                _user.Operadora = operadora;
                await _repositorio.Update(_user);

                await _repositorio.AsignarCanal(new CanalUsuario() { Usuario = _user, Canal = new Canal() { Id = new Guid(_configuracion["CanalBase"]) } });

                await NotificarPorEmail(request);
                return _user.Id;
            }
            else
            {
                throw new ExcepcionApp(result.Errors.ToList()[0].Code);
            }
        }

        private async Task AsignarRoles(CrearUsuarioME request, Usuario _user)
        {
            if (request.Roles != null)
            {
                foreach (var item in request.Roles)
                {
                    var rol = await _repositorioRol.Get(item.Id);
                    await _manejadorUsuario.AddToRoleAsync(_user, rol.NormalizedName);
                }
            }
        }

        private async Task GenerarUsuarioCoreFinanciero(CrearUsuarioME request, Usuario _user)
        {
            if (request.Roles != null && request.Roles.Any(r => r.Id == "e0aee3b1-57c9-4d4e-8f35-4bf3bc38236e"))
            {
                try
                {
                    var respuesta = await _clienteApi.CreaUsuarioAsync(
                        new CreaUsuarioRequest()
                        {
                            CodigoUsuarioCorresponsal = _user.UserName,
                            NombreUsuarioCorresponsal = request.NombreMostrar,
                            Contrasenia = request.Contrasenna,
                            Correo = request.CorreoElectronico,
                            SecuencialCuenta = request.Cuenta
                        });

                    if (respuesta == null || (respuesta is System.Text.Json.JsonElement jsonElement && jsonElement.ValueKind == System.Text.Json.JsonValueKind.False))
                    {
                        throw new ExcepcionApp("No se ha podido crear el usuario en core financiero, por favor inténtelo más tarde.", TipoError.Error);
                    }
                }
                catch (Exception e)
                {
                    throw new ExcepcionApp("No se ha podido crear el usuario en core financiero, por favor inténtelo más tarde.", TipoError.Error);
                }
            }
        }

        private async Task NotificarPorEmail(CrearUsuarioME request)
        {
            try
            {
                var emailTemplate = File.ReadAllText("Resources/EmailTemplate/creacion_usuario.html");

                emailTemplate = emailTemplate.Replace("[:NOMBREUSUARIO:]", request.Usuario)
                       .Replace("[:CONTRASENIA:]", request.Contrasenna)
                       .Replace("[:NOMBRE:]", request.NombreMostrar)
                       .Replace("[:TELEFONO:]", request.Telefono);

                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = "Creación de usuario en la Consola de Administración de Corresponsales Solidarios",
                    Mensaje = emailTemplate,
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = request.CorreoElectronico,
                            Nombre = request.NombreMostrar
                        }
                    }
                });
            }
            catch (Exception e)
            {
                throw new ExcepcionApp("No ha sido posible notificar por correo electrónico el registro del usuario.", TipoError.EmailNotification);
            }
        }
    }
}
