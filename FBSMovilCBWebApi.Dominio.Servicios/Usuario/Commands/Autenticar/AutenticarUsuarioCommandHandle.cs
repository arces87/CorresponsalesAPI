using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class AutenticarUsuarioCommandHandle : IRequestHandler<DatosLoginME, ProcesarLoginMS>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;
        private readonly IRepositorioPersona _repositorioPersona;
        private readonly IConfiguration _configuracion;

        public AutenticarUsuarioCommandHandle(IMediator mediador, IRepositorioPersona repositorioPersona, IMapper mapper, IConfiguration configuracion)
        {
            _mediador = mediador;
            _repositorioPersona = repositorioPersona;
            _mapper = mapper;
            _configuracion = configuracion;
        }

        public async Task<ProcesarLoginMS> Handle(DatosLoginME request, CancellationToken cancellationToken)
        {
            var _usuario = _mapper.Map<LoginUsuarioCommand>(request);
            var usuarioAutenticado = await _mediador.Send(_usuario);
            if (usuarioAutenticado.Errores != null)
            {
                var intentos = int.Parse(_configuracion["IntentosAutenticacion"]);
                if (request.NumeroIntento >= intentos)
                    throw new Exception("Ha excedido el número de intentos fallidos permitidos en el sistema, su usuario ha sido bloqueado");
                throw new Exception(usuarioAutenticado.Errores);
            }
            var usuario = new ProcesarLoginMS()
            {
                GuidID = request.User.GuidID,
                TieneOtp = false,
                EsPrimeraVez = await _repositorioPersona.GetEstadoCorresponsalPorIdUsuario(request.User.UsuarioLogin)
            };

            return usuario;
        }
    }
}
