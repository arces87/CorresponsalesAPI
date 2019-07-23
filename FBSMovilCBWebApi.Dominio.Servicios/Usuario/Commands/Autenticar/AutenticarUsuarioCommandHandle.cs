using AutoMapper;
using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class AutenticarUsuarioCommandHandle : IRequestHandler<AutenticarUsuarioCommand, ModeloUsuarioAutenticado>
    {
        private readonly IServicioUsuario _servicio;
        private readonly IMapper _mapper;

        public AutenticarUsuarioCommandHandle(IServicioUsuario servicio, IMapper mapper)
        {
            _servicio = servicio;
            _mapper = mapper;
        }

        public async Task<ModeloUsuarioAutenticado> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var _usuario = _mapper.Map<ModeloUsuario>(request);
            var usuarioAutenticado = (ModeloUsuario)(await _servicio.Login(_usuario));

            return _mapper.Map<ModeloUsuarioAutenticado>(usuarioAutenticado);
        }
    }
}
