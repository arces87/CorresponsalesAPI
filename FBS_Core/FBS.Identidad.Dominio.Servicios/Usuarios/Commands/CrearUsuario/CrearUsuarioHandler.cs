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
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class CrearUsuarioHandler : IRequestHandler<CrearUsuarioME, string>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IRepositorioRol _repositorioRol;
        private readonly IRepositorioUsuario _repositorio;
        private readonly IConfiguration _configuracion;
        private readonly IClientesApi _clienteApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly byte[] _llave;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public CrearUsuarioHandler(
            UserManager<Usuario> manejadorUsuario, 
            IRepositorioRol repositorioRol,
            IClientesApi clienteApi,
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

                await NotificarPorEmail(_user.UserName, request.CorreoElectronico, request.NombreCompleto, request.Contrasenna);
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
                    var apiKey = _apiKeyGenerator.generateApiKey("000000000000000");
                    var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

                    var respuesta = await _clienteApi.ClientesCreaUsuarioAsync(new PorCodigoUsuarioCorresponsalME() { CodigoUsuarioCorresponsal = _user.UserName });

                    if (!respuesta)
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

        private async Task NotificarPorEmail(string userName, string email, string nombreCompleto, string contrasenna)
        {
            try
            {
                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = "Creación de usuario en la Consola de Administración de Corresponsales Solidarios",
                    Mensaje = "Se ha creado una cuenta con su correo electrónico, con el usuario: " + userName + " y contraseña: " + contrasenna,
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = email,
                            Nombre = nombreCompleto
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
