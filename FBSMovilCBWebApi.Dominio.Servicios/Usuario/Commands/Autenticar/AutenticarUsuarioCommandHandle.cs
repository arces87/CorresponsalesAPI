using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class AutenticarUsuarioCommandHandle : IRequestHandler<AutenticarUsuarioCommand, ModeloUsuarioAutenticadoMovil>
    {
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;

        public AutenticarUsuarioCommandHandle(IMediator mediador, IMapper mapper)
        {
            _mediador = mediador;
            _mapper = mapper;
        }

        public async Task<ModeloUsuarioAutenticadoMovil> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var _usuario = _mapper.Map<LoginUsuarioCommand>(request);
            var usuarioAutenticado = await _mediador.Send(_usuario);

            return _mapper.Map<ModeloUsuarioAutenticadoMovil>(usuarioAutenticado);
        }
    }
}
