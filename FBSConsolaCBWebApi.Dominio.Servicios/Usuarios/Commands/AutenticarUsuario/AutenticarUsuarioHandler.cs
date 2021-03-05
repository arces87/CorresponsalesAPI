using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioHandler : IRequestHandler<AutenticarUsuarioME, AutenticarUsuarioMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;

        public AutenticarUsuarioHandler(IRepositorioAgente repositorio, IMediator mediador, IMapper mapper)
        {
            _repositorio = repositorio;
            _mediador = mediador;
            _mapper = mapper;
        }

        public async Task<AutenticarUsuarioMS> Handle(AutenticarUsuarioME request, CancellationToken cancellationToken)
        {
            var _modelo = _mapper.Map<LoginUsuarioME>(request);
            _modelo.Dispositivo = "Consola";
            var respuesta = await _mediador.Send(_modelo);
            var _usuario = _mapper.Map<AutenticarUsuarioMS>(respuesta);
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
