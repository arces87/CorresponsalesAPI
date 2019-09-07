using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioHandler : IRequestHandler<AutenticarUsuarioME, AutenticarUsuarioMS>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;

        public AutenticarUsuarioHandler(IRepositorioPersona repositorio, IMediator mediador, IMapper mapper)
        {
            _repositorio = repositorio;
            _mediador = mediador;
            _mapper = mapper;
        }

        public async Task<AutenticarUsuarioMS> Handle(AutenticarUsuarioME request, CancellationToken cancellationToken)
        {
            var _modelo = _mapper.Map<LoginUsuarioME>(request);
            var _usuario = _mapper.Map<AutenticarUsuarioMS>(await _mediador.Send(_modelo));
            if (_usuario.Errores == null && _usuario.Errores != "")
            {
                var persona = await _repositorio.GetForUserName(_modelo.Usuario);
                if (persona != null)
                {
                    _mapper.Map(persona, _usuario);
                }
            }

            return _usuario;
        }
    }
}
